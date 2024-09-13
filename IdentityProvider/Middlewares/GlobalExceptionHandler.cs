using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProvider.Middlewares
{
    public class GlobalExceptionHandler 
        (ILogger<GlobalExceptionHandler> logger 
        ): IExceptionHandler
    {     
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails
            {
                Detail = exception.Message,
                Status = StatusCodes.Status500InternalServerError,
                Instance = httpContext.Request.Path,
                Title = "Internal Server Error",
                Type = exception.GetType().FullName,
                Extensions =
                {
                    ["Source"] = exception.Source,
                    ["MethodName"] = exception.TargetSite?.Name
                }
            };
            logger.LogError(problemDetails.ToString());
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
