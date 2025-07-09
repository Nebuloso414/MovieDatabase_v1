using Swashbuckle.AspNetCore.Filters;
using System.Net;
using MovieDatabase.Core.Models;

public class APIResponseNotFoundExample : IExamplesProvider<APIResponse>
{
    public APIResponse GetExamples()
    {
        return new APIResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            IsSuccess = false,
            Errors = new List<string> { "Genre with ID 123 not found." },
            Result = null
        };
    }
}