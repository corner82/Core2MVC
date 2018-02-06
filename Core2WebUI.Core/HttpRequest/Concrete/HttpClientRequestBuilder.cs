using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Core2WebUI.Core.HttpRequest.Abstract;
using Core2WebUI.Core.HttpRequest.Abstract.Token;
using Core2WebUI.Core.Token.Abstract;

namespace Core2WebUI.Core.HttpRequest.Concrete
{
    public class HttpClientRequestBuilder : RequestBuilderBase
    {
        protected  string _acceptHeader = "application/json";
        protected string _token = "";
        ITokenCreater _tokenCreator = null;

        public HttpClientRequestBuilder () :base()
        {
        }

        public string Test()
        {
            return "";
        }


    }
}
