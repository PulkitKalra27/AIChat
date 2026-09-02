namespace AIChat.Models
{
    public class ChatStreamEvent
    {
        public string type { get; set; } = string.Empty;
        public string? conversationId { get; set; }
        public string? content { get; set; }
    }
}