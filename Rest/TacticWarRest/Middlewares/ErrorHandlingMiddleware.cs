using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Exceptions;

namespace TacticWar.Rest.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (GameException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                await WriteErrorResult(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
                await WriteErrorResult(context, HttpStatusCode.InternalServerError, e.Message);
            }
        }


        private static Task WriteErrorResult(HttpContext context, HttpStatusCode code, string message)
        {
            string result = JsonConvert.SerializeObject(new { error = message });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
