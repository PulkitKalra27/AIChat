using AIChat.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AIChat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService _conversationService;
        public ConversationController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetConversationsAsync()
        {
            var conversations = await _conversationService.GetConversationsAsync();
            return Ok(conversations);
        }
        [HttpGet("{conversationId}/messages")]
        public async Task<IActionResult> GetMessagesAsync(Guid conversationId)
        {
            var messages = await _conversationService.GetMessagesAsync(conversationId);
            return Ok(messages);
        }
        
    }
}
