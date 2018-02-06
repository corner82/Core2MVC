using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core2WebUI.Core.Utills;
using Wangkanai.Detection;

namespace Core2WebUI.Filters
{
    public class TestDIAttribute : ActionFilterAttribute
    {
        private readonly RemoteAddressFinder _remoteAddressFinder;
        private readonly IDeviceResolver _deviceResolver;
        public TestDIAttribute(RemoteAddressFinder remoteAdresFinder,
                               IDeviceResolver deviceResolver)

        {
            _remoteAddressFinder = remoteAdresFinder;
            _deviceResolver = deviceResolver;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ip = _remoteAddressFinder.GetRequestIP();
            var device = _deviceResolver.Device;
            var userAgent = _deviceResolver.UserAgent;
            var userAgentString = _deviceResolver.UserAgent.ToString();
            base.OnActionExecuting(context);
        }
    }
}
