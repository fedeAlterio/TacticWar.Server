using Newtonsoft.Json;
using System.Net;
using TacticWar.Lib.Game.Exceptions;

namespace TacticWar.Rest.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        readonly RequestDelegate next;
        readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            this.next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (GameException ex)
            {
                _logger.LogError(ex, "Game exception");
                await WriteErrorResult(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error handling request");
                await WriteErrorResult(context, HttpStatusCode.InternalServerError, e.Message);
            }
        }

        static Task WriteErrorResult(HttpContext context, HttpStatusCode code, string message)
        {
            string result = JsonConvert.SerializeObject(new { error = message });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
