using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core2WebUI.Entities.Session;
using Core2WebUI.Extensions;
using Core2WebUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Core2WebUI.Core.Utills;

namespace Core2WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdmController : Controller
    {
        private readonly IDistributedCache _distributedCache;
        public AdmController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }


        [SessionTimeOut]
        public async Task<IActionResult> Dsh()

        {
            /*_distributedCache.SetString("test","helloFromRedis");
            var bytes = Encoding.UTF8.GetBytes("deneme session value");
            HttpContext.Session.Set("session12", bytes);
            var valueFromRedis2 = default(byte[]);
            if (HttpContext.Session.TryGetValue("session12", out valueFromRedis2))
            {
                var valueToDisplay2 = Encoding.UTF8.GetString(valueFromRedis2);
            }*/
            var user = HttpContext.Session.Get<SessionUserModel>("CurrentUser");
            var ff = JsonConvert.SerializeObject(user);
            _distributedCache.SetString("userTestObj",JsonConvert.SerializeObject(user));
            SessionUserModel model = JsonConvert.DeserializeObject<SessionUserModel>(_distributedCache.GetString("userTestObj"));
            var userName = model.Email;

            var user2 = JsonConvert.DeserializeObject<SessionUserModel>(await _distributedCache.GetStringAsync(user.ConcurrencyStamp));
            var userNmae2 = user2.Email;
            return View();
        }

        [AllowAnonymous]
        //[AjaxOnly]
        [AjaxOnlyOptimized]
        //[AjaxSessionTimeOut]
        [ServiceFilter(typeof(AjaxSessionTimeOutAttribute))]
        public string DshTest()
        {
            return "test";
        }


    }
}