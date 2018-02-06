using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core2WebUI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Core2WebUI.Filters
{
    public  class AjaxOnlyAttribute : ActionMethodSelectorAttribute
    {
        public override  bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            if (!routeContext.HttpContext.Request.IsAjaxRequest()) {
                routeContext.HttpContext.Response.StatusCode = 301;
                return false;
            } else
            {
                return true;
            }
                
        }
    }
}
