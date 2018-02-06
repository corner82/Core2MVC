using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Core2WebUI.Extensions;
using Microsoft.AspNetCore.Mvc;
using Core2WebUI.Core.Utills;
using Core2WebUI.Entities.Session;

namespace Core2WebUI.Filters
{
    public class AjaxSessionTimeOutAttribute : ActionFilterAttribute
    {
        private readonly RemoteAddressFinder _remoteAdresFinder;

        public  AjaxSessionTimeOutAttribute(RemoteAddressFinder remoteAdresFinder) {
            _remoteAdresFinder = remoteAdresFinder;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //_remoteAdresFinder = context.HttpContext.RequestServices.GetService<RemoteAddressFinder>();
            var user = context.HttpContext.Session.Get<SessionUserModel>("CurrentUser");
            if (user == null)
            {
                //throw new ArgumentNullException();
                //context.Result = new BadRequestObjectResult(context.ModelState);
                context.Result = new StatusCodeResult(403);
            }
            context.HttpContext.Session.Set<SessionUserModel>("CurrentUser", user);
            base.OnActionExecuting(context);
        }
    }
}
