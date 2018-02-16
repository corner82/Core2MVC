using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core2WebUI.Core.Exceptions.Custom
{
    public class RedisManagerException : Exception
    {
        public int StatusCode { get; set; }
        public string ContentType { get; set; } = @"text/plain";

        public RedisManagerException(int statusCode)
        {
            this.StatusCode = statusCode;
        }

        public RedisManagerException(int statusCode, string message) : base(message)
        {
            this.StatusCode = statusCode;
        }

        public RedisManagerException(Exception inner) 
        {
            
        }

        public RedisManagerException(int statusCode, Exception inner) : this(statusCode, inner.ToString()) { }

        public RedisManagerException(int statusCode, JObject errorObject) : this(statusCode, errorObject.ToString())
        {
            this.ContentType = @"application/json";
        }
    }
}
