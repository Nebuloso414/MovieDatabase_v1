using System.Net;

namespace MovieDatabase.Core.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> Errors { get; set; } = new();
        public object? Result { get; set; }
    }
}