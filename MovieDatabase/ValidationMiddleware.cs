using FluentValidation;
using MovieDatabase.Core.Models;
using System.Net;

namespace MovieDatabase.Api
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;
        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = 400;
                //context.Response.ContentType = "application/json";
                var response = new APIResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = ex.Errors.Select(e => e.ErrorMessage).ToList()
                };
                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var response = new APIResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string>() { ex.Message }
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
