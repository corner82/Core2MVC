using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Core2WebUI.Core.HttpRequest.Concrete.Content;
using Core2WebUI.Core.Token.Abstract;

namespace Core2WebUI.Core.HttpRequest.Concrete
{
    public class HttpClientRequestFactory
    {
        public static async Task<HttpResponseMessage> Get(string requestUri)
        {
            var builder = new HttpClientRequestBuilder()
                                .AddMethod(HttpMethod.Get)
                                .AddRequestUri(requestUri);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Get(string requestUri, ITokenCreater tokenCreator)
        {
            var builder = new HttpClientRequestBuilder()
                                .AddMethod(HttpMethod.Get)
                                .AddRequestUri(requestUri)
                                .AddTokenCreator(tokenCreator);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Get(string requestUri, Dictionary<string, string> headers, object value)
        {
            var builder = new HttpClientRequestBuilder()
                                .AddMethod(HttpMethod.Get)
                                .AddRequestUri(requestUri)
                                .AddContent(new JsonContent(value))
                                .AddHeaders(headers);
                                //.AddTokenCreator(tokenCreator);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Get(string requestUri, Dictionary<string, string> headers)
        {
            var builder = new HttpClientRequestBuilder()
                                .AddMethod(HttpMethod.Get)
                                .AddRequestUri(requestUri)
                                .AddHeaders(headers);
            //.AddTokenCreator(tokenCreator);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Post(string requestUri, object value)
            => await Post(requestUri, value, null);

        public static async Task<HttpResponseMessage> Post(
            string requestUri, object value, ITokenCreater tokenCreator)
        {
            var builder = new HttpClientRequestBuilder()
                                .AddMethod(HttpMethod.Post)
                                .AddRequestUri(requestUri)
                                .AddContent(new JsonContent(value))
                                .AddTokenCreator(tokenCreator);
            
            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Put(string requestUri, object value)
            => await Put(requestUri, value, null);

        public static async Task<HttpResponseMessage> Put(
            string requestUri, object value, ITokenCreater tokenCreator)
        {
            var builder = new HttpClientRequestBuilder()
                                .AddMethod(HttpMethod.Put)
                                .AddRequestUri(requestUri)
                                .AddContent(new JsonContent(value))
                                .AddTokenCreator(tokenCreator);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Patch(string requestUri, object value)
            => await Patch(requestUri, value, null);

        public static async Task<HttpResponseMessage> Patch(
            string requestUri, object value, ITokenCreater tokenCreator)
        {
            var builder = new HttpClientRequestBuilder()
                                .AddMethod(new HttpMethod("PATCH"))
                                .AddRequestUri(requestUri)
                                .AddContent(new PatchContent(value))
                                .AddTokenCreator(tokenCreator);

            return await builder.SendAsync();
        }

        public  async Task<HttpResponseMessage> Delete(string requestUri)
            => await Delete(requestUri, null);

        public static async Task<HttpResponseMessage> Delete(
            string requestUri, ITokenCreater tokenCreator)
        {
            var builder = new HttpClientRequestBuilder()
                                .AddMethod(HttpMethod.Delete)
                                .AddRequestUri(requestUri)
                                .AddTokenCreator(tokenCreator);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> PostFile(string requestUri,
            string filePath, string apiParamName)
            => await PostFile(requestUri, filePath, apiParamName, null);

        public static async Task<HttpResponseMessage> PostFile(string requestUri,
            string filePath, string apiParamName, ITokenCreater tokenCreator)
        {
            var builder = new HttpClientRequestBuilder()
                                .AddMethod(HttpMethod.Post)
                                .AddRequestUri(requestUri)
                                .AddContent(new FileContent(filePath, apiParamName))
                                .AddTokenCreator(tokenCreator);

            return await builder.SendAsync();
        }

        
}
}
