
namespace AIChat.Models
{
    public class OpenRouterStreamingResponse
    {
        public List<OpenRouterStreamingChoice> Choices { get; set; } = new();
    }
    public class OpenRouterStreamingChoice
    {
        public OpenRouterDelta Delta { get; set; } = new();
    }
    public class OpenRouterDelta
    {
        public string? Role { get; set; }
        public string? Content { get; set; }
    }
}
