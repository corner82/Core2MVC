using System;
using System.Collections.Generic;
using System.Text;

namespace Core2WebUI.Entities.Session
{
    [Serializable()]
    public class SessionUserClaimModel
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
