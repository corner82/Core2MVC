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
        private static  SessionUserModel _sessionUserModel;
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
                _sessionUserModel = _sessionUserModel ?? session.Get<SessionUserModel>("CurrentUser");
                //SessionUserModel user = session.Get<SessionUserModel>("CurrentUser");

                if (_sessionUserModel != null && !string.IsNullOrEmpty(_sessionUserModel.Email))
                {
                    return _sessionUserModel.Email;
                }
                return "";
            } catch (Exception ex) {

            }
            return "";
        }

        public static string GetUserPassword(this ISession session)
        {
            try
            {
                _sessionUserModel = _sessionUserModel ?? session.Get<SessionUserModel>("CurrentUser");

                if (_sessionUserModel != null && !string.IsNullOrEmpty(_sessionUserModel.Password))
                {
                    return _sessionUserModel.Password;
                }
                return "";
            }
            catch (Exception ex)
            {

            }
            return "";
        }

        public static string GetUserEmail(this ISession session)
        {
            try
            {
                _sessionUserModel = _sessionUserModel ?? session.Get<SessionUserModel>("CurrentUser");

                if (_sessionUserModel != null && !string.IsNullOrEmpty(_sessionUserModel.Email))
                {
                    return _sessionUserModel.Email;
                }
                return "";
            }
            catch (Exception ex)
            {

            }
            return "";
        }

        public static string GetUserPrivateKey(this ISession session)
        {
            try
            {
                _sessionUserModel = _sessionUserModel ?? session.Get<SessionUserModel>("CurrentUser");

                if (_sessionUserModel != null && !string.IsNullOrEmpty(_sessionUserModel.SecurityStamp))
                {
                    return _sessionUserModel.SecurityStamp;
                }
                return "";
            }
            catch (Exception ex)
            {

            }
            return "";
        }

        public static string GetUserPublicKey(this ISession session)
        {
            try
            {
                _sessionUserModel = _sessionUserModel ?? session.Get<SessionUserModel>("CurrentUser");

                if (_sessionUserModel != null && !string.IsNullOrEmpty(_sessionUserModel.ConcurrencyStamp))
                {
                    return _sessionUserModel.ConcurrencyStamp;
                }
                return "";
            }
            catch (Exception ex)
            {

            }
            return "";
        }

        public static void SetHmacToken(this ISession session, string hmacToken)
        {
            try
            {

                _sessionUserModel = _sessionUserModel ?? session.Get<SessionUserModel>("CurrentUser");
                _sessionUserModel.HmacToken = hmacToken;
                var token = _sessionUserModel.HmacToken;
            } catch(Exception ex)
            {

            }
        }

        public static string GetHmacToken(this ISession session)
        {
            try {
                _sessionUserModel = _sessionUserModel ?? session.Get<SessionUserModel>("CurrentUser");
                var token = _sessionUserModel.HmacToken;
                if (_sessionUserModel != null && !string.IsNullOrEmpty(_sessionUserModel.HmacToken))
                {
                    return _sessionUserModel.HmacToken;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {

            }
            return "";
        }
    }
}
