using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitLogSubs.Session
{
    [Serializable()]
    public class SessionUserClaimModel
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
