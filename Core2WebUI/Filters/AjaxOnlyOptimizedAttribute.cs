using Core2WebUI.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core2WebUI.Filters
{
    public class AjaxOnlyOptimizedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(!context.HttpContext.Request.IsAjaxRequest())
            {
                context.Result = new StatusCodeResult(405);
            }
                                
        }
    }
}
