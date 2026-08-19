
namespace AIChat.Factories
{
    public interface IOpenRouterRequestFactory
    {
        HttpRequestMessage CreateStreamingRequest(string message);
    }
}
