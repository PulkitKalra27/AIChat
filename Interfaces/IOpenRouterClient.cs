namespace AIChat.Interfaces
{
    public interface IOpenRouterClient
    {
        IAsyncEnumerable<string> StreamChatAsync(string message);
    }
}
