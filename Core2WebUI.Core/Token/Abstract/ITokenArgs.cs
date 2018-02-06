using System;
using System.Collections.Generic;
using System.Text;

namespace Core2WebUI.Core.Token.Abstract
{
    public interface ITokenArgs
    {
        string PublicKey { get; set; }
        string PrivateKey { get; set; }
        string Salt { get; set; }
    }
}
