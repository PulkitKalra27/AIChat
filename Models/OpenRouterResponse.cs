namespace AIChat.Models
{
    public class OpenRouterResponse
    {
        public List<OpenRouterChoice> Choices { get; set; } = new();
    }

    public class OpenRouterChoice
    {
        public OpenRouterMessage Message { get; set; } = new();
    }
}
