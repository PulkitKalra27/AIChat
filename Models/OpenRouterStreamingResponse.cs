
namespace AIChat.Models
{
    public class OpenRouterStreamingResponse
    {
        public List<OpenRouterStreamingChoice> choices { get; set; } = new();
    }
    public class OpenRouterStreamingChoice
    {
        public OpenRouterDelta delta { get; set; } = new();
    }
    public class OpenRouterDelta
    {
        public string? role { get; set; }
        public string? content { get; set; }
    }
}
