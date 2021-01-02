using Newtonsoft.Json;
using OpenQA.Selenium.Remote;
using Yasf.Common.SessionManagement.Contracts;

namespace Yasf.Common.SessionManagement
{
    public class AttachableSeleniumSession : IAttachableSeleniumSession
    {
        public AttachableSeleniumSession()
        {
        }

        public string BrowserName { get; set; }
        public Response Response { get; set; }
        public string OfficialResponse { get; set; }
        public string RemoteServerUri { get; set; }
        public string CommandRepositoryTypeName { get; set; }

        [JsonIgnore()]
        public bool IsValid { get; set; }
    }
}
