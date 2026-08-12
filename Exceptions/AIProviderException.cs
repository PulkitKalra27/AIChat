using System.Net;

namespace AIChat.Exceptions
{
    public class AIProviderException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public AIProviderException (HttpStatusCode statusCode, string message):base (message)
        {
            StatusCode = statusCode;
        }
    }
}
