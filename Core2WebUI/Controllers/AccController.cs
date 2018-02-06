using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core2WebUI.Entities.Identity;
using Core2WebUI.Entities.Session;
using Core2WebUI.Extensions;
using Core2WebUI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Core2WebUI.Controllers
{
    public class AccController : Controller
    {
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly SignInManager<CustomIdentityUser> _signinManager;
        private readonly RoleManager<CustomIdentityRole> _roleManager;
        private readonly IDistributedCache _distributedCache;

        public AccController(UserManager<CustomIdentityUser> userManager,
                                SignInManager<CustomIdentityUser> signinManager,
                                RoleManager<CustomIdentityRole> roleManager,
                                IDistributedCache distributedCache)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public IActionResult Login()
        {
            
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

           
            var result = _signinManager.PasswordSignInAsync(model.Email, 
                                                            model.Password, false, false).Result;

            if(result.Succeeded)
            {
               
                try
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var roles = _userManager.GetRolesAsync(user);

                    var claims = _userManager.GetClaimsAsync(user);
                    List<Claim> claimList = claims.Result.ToList();
                    List<SessionUserClaimModel> sessionUserClaimList = new List<SessionUserClaimModel>();
                    foreach(var claim in claimList)
                    {
                        sessionUserClaimList.Add(new SessionUserClaimModel()
                        {
                            ClaimType = claim.Type,
                            ClaimValue  = claim.Value
                        });
                    }

                    List<string> roleList = roles.Result.ToList();

                    var sessionUser = new SessionUserModel()
                    {
                        Email = user.Email,
                        ConcurrencyStamp = user.ConcurrencyStamp,
                        Id = user.Id,
                        PhoneNumber = user.PhoneNumber,
                        SecurityStamp = user.SecurityStamp,
                        UserName = user.UserName,
                        Roles = roleList,
                        UserClaims = sessionUserClaimList,
                        Password = user.PasswordHash
                    };
                    HttpContext.Session.Set("CurrentUser", sessionUser);
                    await _distributedCache.SetStringAsync(user.ConcurrencyStamp, JsonConvert.SerializeObject(user));

                }
                catch(Exception ex)
                {
                    throw new Exception("unhandled exception", ex);

                }
                return Redirect("~/Adm/Dsh");
            } else
            {
                ModelState.AddModelError("hata", "status not verified");
            } 

            return View(model);
            
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            //return RedirectToAction(nameof(HomeController.Index), "Home");
            return RedirectToAction("Login");
            //return RedirectToAction("Index");

        }
    }
}