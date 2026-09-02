using AIChat.Models;

namespace AIChat.Interfaces
{
    public interface IConversationRepository
    {
        Task<Conversations> CreateConversationAsync(Conversations conversation);
        Task AddMessageAsync(message message);
        Task<List<message>> GetMessagesAsync(Guid conversationId);
        Task<List<Conversations>> GetConversationsAsync();
    }
}
