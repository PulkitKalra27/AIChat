using AIChat.Interfaces;
using AIChat.Models;
using AIChat.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.IO;
using AIChat.Exceptions;
using System.Text.Json;
using AIChat.Factories;


namespace AIChat.Services
{
    public class ChatService : IChatService
    {
        private readonly IOpenRouterClient _openRouterClient;
        private readonly IConversationRepository _conversationRepository;
        public ChatService(IOpenRouterClient openRouterClient, IConversationRepository conversationRepository)
        {
           _openRouterClient = openRouterClient;
           _conversationRepository = conversationRepository;
        }
        //public async Task<ChatResponse> AskQuestionAsync(ChatRequest request)
        //{
        //    //throw new InvalidOperationException("Simulated unexpected error");
        //    var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_options.BaseUrl}/chat/completions");
        //    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);

        //    var openRouterRequest = new OpenRouterRequest
        //    {
        //        Model = _options.Model,
        //        Messages = new List<OpenRouterMessage>
        //        {
        //            new OpenRouterMessage
        //            {
        //                Role = "user",
        //                Content = request.Message
        //            }
        //        }
        //    };
        //    requestMessage.Content = JsonContent.Create(openRouterRequest);

        //    //throw new HttpRequestException("Unable to connect To OpenRouter");
            
        //    try
        //    {
        //        //response.Content = JsonContent.Create(new
        //        //{
        //        //    error = new
        //        //    {
        //        //        message = "Invalid API Key"
        //        //    }
        //        //});
        //        //response.EnsureSuccessStatusCode();
        //        var response = await _httpClient.SendAsync(requestMessage);
        //        //var StatusCode = HttpStatusCode.ServiceUnavailable;
        //        //var response = new HttpResponseMessage(StatusCode);
        //        if (!response.IsSuccessStatusCode)
        //        {
        //            var errorbody = await response.Content.ReadAsStringAsync();
        //            throw new AIProviderException(response.StatusCode, errorbody);
        //        }
        //        var openRouterResponse = await response.Content.ReadFromJsonAsync<OpenRouterResponse>();
        //        var answer = openRouterResponse?.Choices.FirstOrDefault()?.Message.Content;
        //        return new ChatResponse
        //        {
        //            Response = answer ?? string.Empty
        //        };
        //    }
        //    catch(HttpRequestException ex)
        //    {
        //        throw new AIProviderException(HttpStatusCode.ServiceUnavailable, "Unable to communicate with AI Provider.", ex);
        //    }

        //    //return await Task.FromResult(new ChatResponse
        //    //{
        //    //    Response = "Hello from ChatService"
        //    //});
        //}

        public async IAsyncEnumerable<string> StreamQuestionAsync(ChatRequest request)
        {
            Guid conversationId;
            if (request.conversationid==null)
            {
                var conversation = new Conversations
                {
                    id = Guid.NewGuid(),
                    title = request.message,
                    createdAt = DateTime.UtcNow,
                };
                await _conversationRepository.CreateConversationAsync(conversation);
                conversationId = conversation.id;
            }
            else
            {
                conversationId = request.conversationid.Value;
            }
            var userMessage = new message
            {
                id = Guid.NewGuid(),
                conversationid = conversationId,
                role = "user",
                content = request.message,
                createdAt = DateTime.UtcNow,
            };
            await _conversationRepository.AddMessageAsync(userMessage);
            yield break;
            //return _openRouterClient.StreamChatAsync(request.message);
        }
    }
}
