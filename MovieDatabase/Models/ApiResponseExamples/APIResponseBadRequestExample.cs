using Swashbuckle.AspNetCore.Filters;
using System.Net;
using MovieDatabase.Models;

public class APIResponseBadRequestExample : IExamplesProvider<APIResponse>
{
    public APIResponse GetExamples()
    {
        return new APIResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            IsSuccess = false,
            Errors = new List<string> { "Invalid ID provided." },
            Result = null
        };
    }
}