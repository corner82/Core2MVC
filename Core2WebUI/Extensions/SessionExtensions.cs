using Core2WebUI.Entities.Session;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core2WebUI.Extensions
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) :
                            JsonConvert.DeserializeObject<T>(value);
        }

        public static string GetUserName(this ISession session)
        {
            try {
                SessionUserModel user = session.Get<SessionUserModel>("CurrentUser");
               
                if (user != null)
                {
                    return user.Email;
                }
                return "";
            } catch (Exception ex) {

            }
            return "";
        }
    }
}
