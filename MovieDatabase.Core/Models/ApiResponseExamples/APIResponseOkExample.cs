using Swashbuckle.AspNetCore.Filters;
using MovieDatabase.Core.Models;
using System.Net;

public class APIResponseOkExample : IExamplesProvider<APIResponse>
{
    public APIResponse GetExamples()
    {
        return new APIResponse
        {
            StatusCode = HttpStatusCode.OK,
            IsSuccess = true,
            Errors = new List<string>(),
            Result = new object()
        };
    }
}