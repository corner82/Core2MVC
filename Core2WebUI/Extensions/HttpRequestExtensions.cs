using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core2WebUI.Extensions
{
    public static class HttpRequestExtensions
    {

        private const string RequestedWithHeader = "X-Requested-With";
        private const string XmlHttpRequest = "XMLHttpRequest";

        // use this extension method in webui filters or middleware
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                //throw new ArgumentNullException("request");
                return false;
            }

            if (request.Headers != null)
            {
                return request.Headers[RequestedWithHeader] == XmlHttpRequest;
            }

            return false;
        }

        // use this extension method in api calls
        public static bool IsApiCall(this HttpRequest request)
        {
            bool isJson = request.GetTypedHeaders().Accept.Contains(
                new MediaTypeHeaderValue("application/json"));
            if (isJson)
                return true;
            if (request.Path.Value.StartsWith("/api/"))
                return true;
            return false;
        }
    }
}
