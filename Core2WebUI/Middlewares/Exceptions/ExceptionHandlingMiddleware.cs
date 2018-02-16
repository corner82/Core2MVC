using Core2WebUI.Core.Exceptions.Custom;
using Core2WebUI.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Core2WebUI.Middlewares.Exceptions
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            /*IActionResult result;
            //result = new JsonResult(ex) { StatusCode = context.Response.StatusCode };
            result = new JsonResult(ex) { StatusCode = 404 };
            RouteData routeData = context.GetRouteData();
            ActionDescriptor actionDescriptor = new ActionDescriptor();
            ActionContext actionContext = new ActionContext(context, routeData, actionDescriptor);*/

            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            if (exception is HttpStatusCodeException) code = HttpStatusCode.NotFound;
            else if (exception is IdentityManagerException) code = HttpStatusCode.Unauthorized;
            else if (exception is RedisManagerException) code = HttpStatusCode.ServiceUnavailable;
            else if(exception is System.Net.Sockets.SocketException) code = HttpStatusCode.BadGateway;
            else if (exception is System.Net.Sockets.SocketException) code = HttpStatusCode.BadGateway;
            else if (exception is NullReferenceException) code = HttpStatusCode.Gone;
            else if (exception is ArgumentNullException) code = HttpStatusCode.LengthRequired;
            else if (exception is ArgumentOutOfRangeException) code = HttpStatusCode.LengthRequired;
            else if (exception is StackOverflowException) code = HttpStatusCode.RequestedRangeNotSatisfiable;
            else if (exception is InvalidOperationException) code = HttpStatusCode.MethodNotAllowed;
            else if (exception is System.Net.Sockets.SocketException) code = HttpStatusCode.BadGateway;

            var result = JsonConvert.SerializeObject(new { error = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
