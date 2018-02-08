using Core2WebUI.Core.Hmac;
using Core2WebUI.Core.Utills;
using Core2WebUI.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wangkanai.Detection;

namespace Core2WebUI.Filters
{
    public class HmacTokenGeneratorAttribute : ActionFilterAttribute
    {
        //private readonly HttpContext _context;
        private readonly IDeviceResolver _deviceResolver;
        private readonly RemoteAddressFinder _remoteAdressFinder;
        public HmacTokenGeneratorAttribute(IDeviceResolver deviceResolver,
                                           RemoteAddressFinder remoteAdressFinder)
        {
            //_context = context;
            _deviceResolver = deviceResolver;
            _remoteAdressFinder = remoteAdressFinder;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            var userName = context.HttpContext.Session.GetUserName();
            var password = context.HttpContext.Session.GetUserPassword();
            /*var userName = _context.Session.GetUserName();
            var password = _context.Session.GetUserPassword();*/
            var ip = _remoteAdressFinder.GetRequestIP();
            var device = _deviceResolver.Device;
            var userAgent = _deviceResolver.UserAgent;
            var userAgentString = _deviceResolver.UserAgent.ToString();
            //TimeSpan.FromMinutes(2).Ticks;
            var ticks = DateTime.Now.Ticks;
            var token = HmacServiceManager.GenerateToken(userName, password
                                                            , ip
                                                            , userAgentString
                                                            , ticks);
            context.HttpContext.Session.SetHmacToken(token);
            base.OnActionExecuting(context);
        }
    }
}
