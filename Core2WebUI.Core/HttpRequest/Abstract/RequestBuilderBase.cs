using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Core2WebUI.Core.Extensions.List;
using Core2WebUI.Core.HttpRequest.Abstract.Token;
using Core2WebUI.Core.Token.Abstract;

namespace Core2WebUI.Core.HttpRequest.Abstract
{
    public abstract class RequestBuilderBase : IRequestBuilder, IReguestTokenProvider
    {

        protected HttpMethod _method = null;
        protected string _requestUri = "";
        protected HttpContent _content = null;
        protected string _acceptHeader = "";
        protected ITokenCreater _tokenCreator = null;
        protected bool _allowAutoRedirect = false;
        protected TimeSpan _timeout = new TimeSpan(0, 0, 15);
        protected Dictionary<string, string> _headers;

        public RequestBuilderBase AddContent(HttpContent content)
        {
            this._content = content;
            return this;
        }

        public RequestBuilderBase AddHeader(string header)
        {
            this._acceptHeader = header;
            return this;
        }

        public RequestBuilderBase AddMethod(HttpMethod method)
        {
            this._method = method;
            return this;
        }

        public RequestBuilderBase AddRequestUri(string requestUri)
        {
            this._requestUri = requestUri;
            return this;
        }

        public RequestBuilderBase AddHeaders(Dictionary<string, string> headers)
        {
            this._headers = headers;
            return this;
        }

        public RequestBuilderBase AddTokenCreator(ITokenCreater tokenCreator)
        {
            this._tokenCreator = tokenCreator;
            return this;
        }

        public RequestBuilderBase AddAllowAutoRedirect(bool allowAutoRedirect)
        {
            this._allowAutoRedirect = allowAutoRedirect;
            return this;
        }

        public RequestBuilderBase AddTimeout(TimeSpan timeout)
        {
            this._timeout = timeout;
            return this;
        }

        public async Task<HttpResponseMessage> SendAsync()
        {
            // Check required arguments
            //EnsureArguments();

            // Set up request
            var request = new HttpRequestMessage
            {
                Method = this._method,
                RequestUri = new Uri(this._requestUri)
            };

            if (this._content != null)
                request.Content = this._content;

            if (this._tokenCreator != null)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this._tokenCreator.CreateToken());
            //request.Headers.Authorization = this._tokenCreator.CreateToken();

            request.Headers.Accept.Clear();
            if (!string.IsNullOrEmpty(this._acceptHeader))
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(this._acceptHeader));


            try {
                if (!this._headers.IsNullOrEmpty())
                {
                    foreach (KeyValuePair<string, string> head in _headers)
                    {
                        request.Headers.Add(head.Key, head.Value);
                    }
                }
            }
            catch(Exception ex)
            {

            }
            

            // Setup client
            var handler = new HttpClientHandler();
            handler.AllowAutoRedirect = this._allowAutoRedirect;

            var client = new System.Net.Http.HttpClient(handler);
            //client.Timeout = this.timeout;

            return await client.SendAsync(request);
        }
    }
}
