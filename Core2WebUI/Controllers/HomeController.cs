using System;
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
using RabbitMQ.Client;
using System.Text;
using Core2WebUI.Core.RabbitMQ;

namespace Core2WebUI.Controllers
{
    public class HomeController : Controller
    {

        private readonly IStringLocalizer _localizer;
        public HomeController(IStringLocalizer localizer)
        {
            _localizer = localizer;
        }

        [SessionTimeOut]
        [ServiceFilter(typeof(HmacTokenGeneratorAttribute))]
        [ServiceFilter(typeof(PageEntryLogRabbitMQAttribute))]
        public async Task<string> Index()
        {
            var headers = new Dictionary<string, string>();
            var tokenGenerated = HttpContext.Session.GetHmacToken();    
            headers.Add("X-Hmac", tokenGenerated);
            headers.Add("X-PublicKey", HttpContext.Session.GetUserPublicKey());
            //_hmacManager.test();
            /*var response = await HttpClientRequestFactory.Get("http://localhost:58443/api/values/23", headers);
            var data = response.Content.ReadAsStringAsync().Result;
            return data.ToString();*/
            return "test";
        }
    }
}