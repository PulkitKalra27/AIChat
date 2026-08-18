namespace AIChat.Models
{
    public class OpenRouterStreamingRequest
    {        
        public string Model { get; set; } = string.Empty;
        public List<OpenRouterMessage> Messages { get; set; } = new();
        public bool Stream { get; set; }
    }

}
