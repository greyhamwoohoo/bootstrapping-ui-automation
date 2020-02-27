using Newtonsoft.Json;
using TheInternet.Common.ExecutionContext.Runtime.BrowserSettings.Contracts;

namespace TheInternet.Common.ExecutionContext.Runtime.BrowserSettings
{
    public class BrowserSettingsBase : IBrowserProperties
    {
        [JsonProperty("browserName")]
        public string BrowserName { get; set; }

        public BrowserSettingsBase BrowserSettings => this;

        string IBrowserProperties.Name => BrowserName;
    }
}
