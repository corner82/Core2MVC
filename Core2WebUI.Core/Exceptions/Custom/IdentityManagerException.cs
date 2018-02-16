using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core2WebUI.Core.Exceptions.Custom
{
    public class IdentityManagerException : Exception
    {
        public int StatusCode { get; set; }
        public string ContentType { get; set; } = @"text/plain";

        public IdentityManagerException(int statusCode)
        {
            this.StatusCode = statusCode;
        }

        public IdentityManagerException(int statusCode, string message) : base(message)
        {
            this.StatusCode = statusCode;
        }

        public IdentityManagerException(int statusCode, Exception inner) : this(statusCode, inner.ToString()) { }

        public IdentityManagerException(int statusCode, JObject errorObject) : this(statusCode, errorObject.ToString())
        {
            this.ContentType = @"application/json";
        }
    }
}
