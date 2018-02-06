using System;
using System.Collections.Generic;
using System.Text;
using Core2WebUI.Core.Token.Abstract;

namespace Core2WebUI.Core.Token.Concrete
{
    class TokenCreatorHMAC : ITokenCreater, ITokenArgs
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string Salt { get; set; }

        public string CreateToken()
        {
            throw new NotImplementedException();
        }
    }
}
