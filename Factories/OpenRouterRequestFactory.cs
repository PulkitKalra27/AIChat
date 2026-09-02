using System.Net.Http.Headers;
using AIChat.Configuration;
using AIChat.Models;
using Microsoft.Extensions.Options;

namespace AIChat.Factories
{
    public class OpenRouterRequestFactory : IOpenRouterRequestFactory
    {
        private readonly AIOptions _options;

        public OpenRouterRequestFactory(IOptions<AIOptions> options)
        {
            _options = options.Value;
        }
        
        public HttpRequestMessage CreateStreamingRequest(List<OpenRouterMessage> messages)
        {
            var streamingRequest = new OpenRouterStreamingRequest
            {
                model = _options.Model,
                messages = messages,
                stream = true
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post,"chat/completions");
            requestMessage.Content = JsonContent.Create(streamingRequest);
            return requestMessage;
        }
    }
}
