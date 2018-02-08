﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core2WebUI.Entities.Session;
using Core2WebUI.Extensions;
using Core2WebUI.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Core2WebUI.Core.Hmac;
using Core2WebUI.Core.HttpRequest.Concrete;
using Core2WebUI.Core.Utills;
using Wangkanai.Detection;

namespace Core2WebUI.Controllers
{
    public class HomeController : Controller
    {

        private readonly IStringLocalizer _localizer;
        private readonly HmacServiceManager _hmacManager;
        private readonly RemoteAddressFinder _remoteAdresFinder;
        private readonly IDeviceResolver _deviceResolver; 
        public HomeController(IStringLocalizer localizer,
                                        RemoteAddressFinder remoteAdresFinder,
                                        IDeviceResolver deviceResolver
                                        /*, HmacServiceManager hmacManager*/)
        {
            _localizer = localizer;
            _remoteAdresFinder = remoteAdresFinder;
            _deviceResolver = deviceResolver;
            //_hmacManager = hmacManager;
        }

        [SessionTimeOut]
        [ServiceFilter(typeof(TestDIAttribute))]
        [ServiceFilter(typeof(HmacTokenGeneratorAttribute))]
        public async  Task<string> Index()
        {
            var user = HttpContext.Session.Get<SessionUserModel>("CurrentUser");
            var userName = HttpContext.Session.GetUserName();
            var ip = _remoteAdresFinder.GetRequestIP();
            var device = _deviceResolver.Device;
            var userAgent = _deviceResolver.UserAgent;
            var userAgentString = _deviceResolver.UserAgent.ToString();
            //TimeSpan.FromMinutes(2).Ticks;
            var ticks = DateTime.Now.Ticks;
            var headers = new Dictionary<string, string>();
            var tokenGenerated = HttpContext.Session.GetHmacToken();
            if (user != null)
            {
                var token = HmacServiceManager.GenerateToken(user.Email,user.Password
                                                            ,_remoteAdresFinder.GetRequestIP()
                                                            ,userAgentString
                                                            , ticks);
                
                headers.Add("X-hash", "hashvalue");
                headers.Add("X-Hmac", HttpContext.Session.GetHmacToken());
                headers.Add("X-PublicKey", user.ConcurrencyStamp);
                //_hmacManager.test();
                //var response = await HttpClientRequestFactory.Get("http://localhost:58443/api/values/23", headers);
                //var data = response.Content.ReadAsStringAsync().Result;
                //return data.ToString();
            }
             return "test";
            //return response.StatusCode.ToString();

        }
    }
}