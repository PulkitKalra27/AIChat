namespace AIChat.Models
{
    public class ChatRequest
    {
        public Guid? conversationid { get; set; }
        public string message { get; set; } = string.Empty;
    }
}
    