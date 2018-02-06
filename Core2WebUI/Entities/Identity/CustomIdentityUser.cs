using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core2WebUI.Entities.Identity
{
    public class CustomIdentityUser : IdentityUser
    {
        //public string UserName { get; set; }
        //public string Email { get; set; }
        public static explicit operator CustomIdentityUser(ClaimsPrincipal v)
        {
            throw new NotImplementedException();
        }
    }
}
