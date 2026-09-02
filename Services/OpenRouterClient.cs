using System.Text.Json;
using AIChat.Factories;
using AIChat.Interfaces;
using AIChat.Models;

namespace AIChat.Services
{
    public class OpenRouterClient : IOpenRouterClient
    {
        //keeps BaseUrl and authentication configuration outside this client
        public readonly HttpClient _httpClient;
        public readonly IOpenRouterRequestFactory _requestFactory;

        public OpenRouterClient( HttpClient httpClient, IOpenRouterRequestFactory requestFactory)
        {
            _httpClient = httpClient;
            _requestFactory = requestFactory;
        }
        public async IAsyncEnumerable<string> StreamChatAsync(List<OpenRouterMessage> messages)
        {
            var requestMessage = _requestFactory.CreateStreamingRequest(messages);
            //starts the processing as soon as the response header arrives.
            var response = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
            var stream = await response.Content.ReadAsStreamAsync();
            // using var calls dispose method auto to clean memory
            using var reader = new StreamReader(stream); //decodes from bytes to readable text
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data:")) // 1st allows spaces and 2nd check the SSE data contain or not
                {
                    continue;
                }
                var data = line["data:".Length..].Trim(); //removes data: and leaves choices{}
                if (data == "[DONE]")
                {
                    break;
                }
                var chunk = JsonSerializer.Deserialize<OpenRouterStreamingResponse>(data); //JsonSerializerOptions - PropertyNameCaseInsensitive also removed

                var content = chunk?.choices.FirstOrDefault()?.delta?.content;
                if (!string.IsNullOrEmpty(content))
                {
                    yield return content;
                }
            }
        }
    }
}
