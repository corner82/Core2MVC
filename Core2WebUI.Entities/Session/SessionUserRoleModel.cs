using System;
using System.Collections.Generic;
using System.Text;

namespace Core2WebUI.Entities.Session
{
    [Serializable()]
    public class SessionUserRoleModel
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
    }
}
