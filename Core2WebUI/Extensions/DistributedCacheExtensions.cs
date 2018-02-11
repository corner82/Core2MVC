using Core2WebUI.Entities.Session;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core2WebUI.Extensions
{
    public static  class DistributedCacheExtensions
    {
        public static async  Task<bool> GetUserByPublicKey(this IDistributedCache cache, string publicKey)
        {
            var user = await cache.GetStringAsync(publicKey);
            var userModel = JsonConvert.DeserializeObject<SessionUserModel>(user);
            if(userModel == null)
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}
