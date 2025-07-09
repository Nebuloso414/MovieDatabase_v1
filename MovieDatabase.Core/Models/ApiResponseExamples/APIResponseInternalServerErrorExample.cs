using Swashbuckle.AspNetCore.Filters;
using MovieDatabase.Core.Models;
using System.Net;
using System.Collections.Generic;

public class APIResponseInternalServerErrorExample : IExamplesProvider<APIResponse>
{
    public APIResponse GetExamples()
    {
        return new APIResponse
        {
            StatusCode = HttpStatusCode.InternalServerError,
            IsSuccess = false,
            Errors = new List<string> { "An unexpected error occurred." },
            Result = null
        };
    }
}