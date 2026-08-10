using AIChat.Interfaces;
using AIChat.Models;
using AIChat.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AIChat.Services
{
    public class ChatService : IChatService
    {
        private readonly HttpClient _httpClient;
        private readonly AIOptions _options;

        public ChatService(HttpClient httpClient, IOptions<AIOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }
        public async Task<ChatResponse> AskQuestionAsync(ChatRequest request)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_options.BaseUrl}/chat/completions");
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _options.ApiKey);

            var openRouterRequest = new OpenRouterRequest
            {
                Model = _options.Model,
                Messages = new List<OpenRouterMessage>
                {
                    new OpenRouterMessage
                    {
                        Role = "user",
                        Content = request.Message
                    }
                }
            };
            requestMessage.Content = JsonContent.Create(openRouterRequest);
            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            var openRouterResponse = await response.Content.ReadFromJsonAsync<OpenRouterResponse>();
            var answer = openRouterResponse?.Choices.FirstOrDefault()?.Message.Content;
            return new ChatResponse
            {
                Response = answer ?? string.Empty
            };
            //return await Task.FromResult(new ChatResponse
            //{
            //    Response = "Hello from ChatService"
            //});
        }
    }
}
