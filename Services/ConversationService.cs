using AIChat.Interfaces;
using AIChat.Models;

namespace AIChat.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository _conversationRepository;
        public ConversationService(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }

        public async Task<List<Conversations>> GetConversationsAsync()
        {
            return await _conversationRepository.GetConversationsAsync();
        }

        public async Task<List<message>> GetMessagesAsync(Guid conversationId)
        {
            return await _conversationRepository.GetMessagesAsync(conversationId);
        }
    }
}
