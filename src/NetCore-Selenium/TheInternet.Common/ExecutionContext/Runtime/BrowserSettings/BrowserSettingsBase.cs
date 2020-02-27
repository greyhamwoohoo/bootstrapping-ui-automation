using Newtonsoft.Json;
using TheInternet.Common.ExecutionContext.Runtime.BrowserSettings.Contracts;

namespace TheInternet.Common.ExecutionContext.Runtime.BrowserSettings
{
    public class BrowserSettingsBase : IBrowserName
    {
        [JsonProperty("browserName")]
        public string BrowserName { get; internal set; }

        string IBrowserName.Value => BrowserName;
    }
}
