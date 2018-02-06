using Core2WebUI.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core2WebUI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        public async Task Invoke(HttpContext context)
        {
            /*try
            {
                //await _next.Invoke(context);
                // SKIPPED. The same as before 
            }
            catch (Exception ex)
            {
                //var resultException = _handler.HandleException(ex, context.Request);
                if (context.Request.IsApiCall())
                {
                    IActionResult result;
                    if (resultException.ClientData != null)
                    {
                        result = new JsonResult(resultException.ClientData) { StatusCode = context.Response.StatusCode };
                    }
                    else
                    {
                        result = new ObjectResult(resultException) { StatusCode = context.Response.StatusCode };
                    }
                    // now we have a IActionResult, let's return it
                    RouteData routeData = context.GetRouteData();
                    ActionDescriptor actionDescriptor = new ActionDescriptor();
                    ActionContext actionContext = new ActionContext(context, routeData, actionDescriptor);
                    await result.ExecuteResultAsync(actionContext);
                    return;
                }
                throw;
            }*/
        }
    }
}
