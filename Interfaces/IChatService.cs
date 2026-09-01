using AIChat.Models;

namespace AIChat.Interfaces
{
    public interface IChatService
    {
        //Task<ChatResponse> AskQuestionAsync(ChatRequest request);
        IAsyncEnumerable<ChatStreamEvent> StreamQuestionAsync(ChatRequest request);
    }
}
