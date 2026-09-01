
using AIChat.Models;

namespace AIChat.Factories
{
    public interface IOpenRouterRequestFactory
    {
        HttpRequestMessage CreateStreamingRequest(List<OpenRouterMessage> messages);
    }
}
