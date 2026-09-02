namespace AIChat.Models
{
    public class OpenRouterStreamingRequest
    {        
        public string model { get; set; } = string.Empty;
        public List<OpenRouterMessage> messages { get; set; } = new();
        public bool stream { get; set; }
    }
    public class OpenRouterMessage
    {
        public string role { get; set; } = string.Empty;
        public string content { get; set; } = string.Empty;
    }

}
