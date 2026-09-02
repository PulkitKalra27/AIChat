using AIChat.Models;

namespace AIChat.Interfaces
{
    public interface IConversationService
    {
        Task<List<Conversations>> GetConversationsAsync();
        Task<List<message>> GetMessagesAsync(Guid conversationId);
    }
}
