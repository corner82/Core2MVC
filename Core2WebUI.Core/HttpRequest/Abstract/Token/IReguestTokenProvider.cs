using System;
using System.Collections.Generic;
using System.Text;
using Core2WebUI.Core.HttpRequest.Concrete;
using Core2WebUI.Core.Token.Abstract;

namespace Core2WebUI.Core.HttpRequest.Abstract.Token
{
    public interface IReguestTokenProvider
    {
        RequestBuilderBase AddTokenCreator(ITokenCreater tokenCreator);
    }
}
