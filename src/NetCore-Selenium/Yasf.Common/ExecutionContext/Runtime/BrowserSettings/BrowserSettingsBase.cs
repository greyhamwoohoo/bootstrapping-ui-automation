using Newtonsoft.Json;
using Yasf.Common.ExecutionContext.Runtime.BrowserSettings.Contracts;

namespace Yasf.Common.ExecutionContext.Runtime.BrowserSettings
{
    public class BrowserSettingsBase : IBrowserProperties
    {
        [JsonProperty("browserName")]
        public string BrowserName { get; set; }

        string IBrowserProperties.Name => BrowserName;

        BrowserSettingsBase IBrowserProperties.BrowserSettings => this;
    }
}
