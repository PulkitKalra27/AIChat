using AIChat.Models;

namespace AIChat.Interfaces
{
    public interface IChatService
    {
        Task<ChatResponse> AskQuestionAsync(ChatRequest request);
    }
}
