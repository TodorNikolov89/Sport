namespace Sport.Web.MIddlewares
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILoggerFactory logger)
        {
            this.next = next;
            this.logger = logger.CreateLogger("MyMiddleware");
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            logger.LogInformation("MyMiddleware executing..");
            await next(httpContext);
        }
    }
}
