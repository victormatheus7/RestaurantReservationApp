using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebAPI.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ControllerBase> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ControllerBase> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (ex is Domain.Exceptions.DomainException || ex is Application.Exceptions.ApplicationException)
                    await HandleCustomExceptionAsync(context, ex);
                else
                {
                    _logger.LogError(ex, "An error occurred");
                    throw;
                }
            }
        }

        private static Task HandleCustomExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return context.Response.WriteAsJsonAsync(new { error = exception.Message });
        }
    }
}
