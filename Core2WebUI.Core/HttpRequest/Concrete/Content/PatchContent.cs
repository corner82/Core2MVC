﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Core2WebUI.Core.HttpRequest.Concrete.Content
{
    
    public class PatchContent : StringContent
    {
        public PatchContent(object value)
            : base(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json-patch+json")
        {
        }
    }
    
}
