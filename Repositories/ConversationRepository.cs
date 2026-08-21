using AIChat.Data;
using AIChat.Interfaces;
using AIChat.Models;
using Microsoft.EntityFrameworkCore;

namespace AIChat.Repositories
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly AppDbContext _context;

        public ConversationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Conversations> CreateConversationAsync(Conversations conversation)
        {
            await _context.Conversations.AddAsync(conversation);
            await _context.SaveChangesAsync();
            return conversation;
        }

        public async Task AddMessageAsync(message message)
        {
            await _context.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task<List<message>> GetMessagesAsync(Guid conversationId)
        {
            return await _context.message
                .Where(message => message.conversationid == conversationId)
                .OrderBy(message => message.createdAt).ToListAsync();
        }
    }
}
