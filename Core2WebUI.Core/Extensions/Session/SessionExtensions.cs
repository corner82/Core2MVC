using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core2WebUI.Core.Session
{
    public static class SessionExtensions 
    {
        /*public static void SetCore<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetCore<T>(this ISession session , string key)
        {
            var value = session.GetString(key);
                return value == null ? default(T) :
                              JsonConvert.DeserializeObject<T>(value);
        }*/
    }
}
