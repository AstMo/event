using bgTeam;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PartyMaker.Application.Exception
{
    public class AppMiddlewareException
    {
        private readonly IAppLogger _logger;
        private readonly RequestDelegate _next;

        public AppMiddlewareException(RequestDelegate next, IAppLogger logger)
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
            catch (AggregateException exp)
            {
                await HandleExceptionAsync(context, exp.GetBaseException(), HttpStatusCode.InternalServerError);
            }
            catch (System.Exception exp)
            {
                await HandleExceptionAsync(context, exp, HttpStatusCode.InternalServerError);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, System.Exception exp, HttpStatusCode code)
        {
            _logger?.Error(exp);

            var result = JsonConvert.SerializeObject(new
            {
                Code = code,
                exp.Message,
                exp.StackTrace,
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
