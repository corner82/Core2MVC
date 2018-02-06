using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Core2WebUI.Core.HttpRequest.Abstract
{
    public interface IRequestBuilder
    {
        RequestBuilderBase AddMethod(HttpMethod method);
        RequestBuilderBase AddRequestUri(string requestUri);
        RequestBuilderBase AddContent(HttpContent content);
        RequestBuilderBase AddHeader(string header);
        RequestBuilderBase AddAllowAutoRedirect(bool allowAutoRedirect);
        RequestBuilderBase AddTimeout(TimeSpan timeout);
    }
}
