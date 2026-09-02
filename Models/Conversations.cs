namespace AIChat.Models
{
    public class Conversations
    {
        public Guid id { get; set; }
        public string title { get; set; } = string.Empty;
        public DateTime createdAt { get; set; }
        public List<message> message { get; set; } = new();
    }
}
