using AIChat.Interfaces;
using AIChat.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIChat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }
        [HttpPost]
        public async Task<ActionResult<ChatResponse>> AskQuestionAsync(ChatRequest request)
        {
            var response = await _chatService.AskQuestionAsync(request);
            return Ok(response);
        }

        [HttpPost("stream")]
        public async Task StreamQuestionAsync(ChatRequest request)
        {
            Response.ContentType = "text/event-stream";
            await foreach(var chunk in _chatService.StreamQuestionAsync(request))
            {
                await Response.WriteAsync($"data: {chunk}\n\n");
                await Response.Body.FlushAsync();
            }
        }

    }
}
