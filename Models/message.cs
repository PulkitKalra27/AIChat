namespace AIChat.Models
{
    public class message
    {
        public Guid id { get; set; }
        public Guid conversationid { get; set; }
        public string role { get; set; } = string.Empty;
        public string content { get; set; } = string.Empty;
        public DateTime createdAt { get; set; }
    }
}
