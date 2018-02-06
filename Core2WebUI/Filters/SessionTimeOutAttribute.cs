using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Core2WebUI.Extensions;
using Core2WebUI.Entities.Session;

namespace Core2WebUI.Filters
{
    public class SessionTimeOutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = filterContext.HttpContext.Session.Get<SessionUserModel>("CurrentUser");
            if ( user == null)
            {
                filterContext.Result = new RedirectResult("~/Acc/Login");
                return;
            } else
            {
                filterContext.HttpContext.Session.Set<SessionUserModel>("CurrentUser", user);
            }
            base.OnActionExecuting(filterContext);
        }

    }
}
