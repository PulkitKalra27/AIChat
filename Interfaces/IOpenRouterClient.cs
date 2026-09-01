using AIChat.Models;

namespace AIChat.Interfaces
{
    public interface IOpenRouterClient
    {
        IAsyncEnumerable<string> StreamChatAsync(List<OpenRouterMessage> messages);
    }
}
