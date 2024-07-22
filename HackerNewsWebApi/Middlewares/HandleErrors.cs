using Newtonsoft.Json;
using System.Net;

namespace HackerNewsWebApi.Middlewares
{
    public class HandleErrors
    {
        private readonly RequestDelegate _next;
        public HandleErrors(RequestDelegate requestDelegate)
        {
            _next=requestDelegate;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError; // 500 if unexpected

            if (exception is ArgumentException)
            {
                statusCode = HttpStatusCode.BadRequest; // 400
            }
            else if (exception is UnauthorizedAccessException)
            {
                statusCode = HttpStatusCode.Unauthorized; // 401
            }
            // Add more specific handling for other exceptions as needed

            var response = new { message = exception.Message };
            var payload = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(payload);
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<HandleErrors>();
        }
    }
}
