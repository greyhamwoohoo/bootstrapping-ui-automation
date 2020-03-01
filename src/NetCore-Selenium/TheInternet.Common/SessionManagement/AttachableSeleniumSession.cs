﻿using Newtonsoft.Json;
using OpenQA.Selenium.Remote;
using TheInternet.Common.SessionManagement.Contracts;

namespace TheInternet.Common.SessionManagement
{
    public class AttachableSeleniumSession : IAttachableSeleniumSession
    {
        public AttachableSeleniumSession()
        {
        }

        public string BrowserName { get; set; }
        public Response Response { get; set; }
        public string RemoteServerUri { get; set; }

        [JsonIgnore()]
        public bool IsValid { get; set; }
    }
}
