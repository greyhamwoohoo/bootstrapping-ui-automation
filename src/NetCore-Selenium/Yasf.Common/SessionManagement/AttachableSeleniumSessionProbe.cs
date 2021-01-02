using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Yasf.Common.SessionManagement.Contracts;

namespace Yasf.Common.SessionManagement
{
    public class AttachableSeleniumSessionProbe : IAttachableSeleniumSessionProbe
    {
        // NOTE: There is no easy way to DI this into the Attachable*Driver context
        public const int PROBE_TIMEOUT = 5000;

        public bool IsRunning(IAttachableSeleniumSession attachableSeleniumSession)
        {
            if (null == attachableSeleniumSession) return false;
            if (!attachableSeleniumSession.IsValid) return false;

            var isDriverReady = IsDriverReady(attachableSeleniumSession);
            if (!isDriverReady) return false;

            var isDriverReallyAbleToTalkToItsBrowser = IsTheDriverReallyReallyReallyAbleToTalkToItsBrowser(attachableSeleniumSession);
            if (!isDriverReallyAbleToTalkToItsBrowser) return false;

            return true;
        }

        private bool IsTheDriverReallyReallyReallyAbleToTalkToItsBrowser(IAttachableSeleniumSession attachableSeleniumSession)
        {
            // UNFORTUNATELY: Just because (say) chromedriver.exe says it is ready does not mean it is actually able to do anything. 
            // chromedriver.exe will spawn multiple chrome.exe processes.
            // If we close the browser (manually), then the browsers go away - but chromedriver.exe is left orphaned. 
            //    Worse, chromedriver.exe will return ready:true in the IsChromeDriverReady check (technically: this is correct; but its misleading)
            //
            // The only way we can really know if a chromedriver.exe can talk to a browser is to issue it a command
            //    Unfortunately, this takes 10-20 seconds to timeout: so instead I set a hard limit here defined by PROBE_TIMEOUT
            //       The assumption being that if it hasn't responded in that time, the browsers are likely dead. 
            //       On a local machine, the response is usually in the order of a few hundred milliseconds. 
            //       This might cause problems when attaching to remotely running browsers due to latency.
            try
            {
                var client = new RestSharp.RestClient(attachableSeleniumSession.RemoteServerUri);
                var request = new RestSharp.RestRequest($"/session/{attachableSeleniumSession.Response.SessionId}/url", RestSharp.Method.GET);
                request.Timeout = 5000;

                var response = client.Execute(request);

                return response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }

        private bool IsDriverReady(IAttachableSeleniumSession attachableSeleniumSession)
        {
            try
            {
                var client = new RestSharp.RestClient(attachableSeleniumSession.RemoteServerUri);
                var request = new RestSharp.RestRequest("/status", RestSharp.Method.GET);

                var response = client.Execute(request);
                // 0 means there is nothing on the port - it is definitely not running
                if (response.StatusCode == 0) return false;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // The status looks something like this:
                    // I look solely for the 'ready = true' field

                    //"value": {
                    //	"build": {
                    //		"version": "80.0.3987.106 (f68069574609230cf9b635cd784cfb1bf81bb53a-refs/branch-heads/3987@{#882})"
                    //	},
                    //	"message": "ChromeDriver ready for new sessions.",
                    //	"os": {
                    //		"arch": "x86_64",
                    //		"name": "Windows NT",
                    //		"version": "10.0.18362"
                    //	},
                    //	"ready": true
                    //}
                    var resultAsJObject = JsonConvert.DeserializeObject<JObject>(response.Content);
                    if (resultAsJObject["value"]["ready"].Type != JTokenType.Boolean)
                    {
                        return false;
                    }

                    // NOTE: We cannot rely on this as true/false - it seems to vary between browsers. 
                    // if (!((bool)resultAsJObject["value"]["ready"]))
                    // {
                    //     return false;
                    // }

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
