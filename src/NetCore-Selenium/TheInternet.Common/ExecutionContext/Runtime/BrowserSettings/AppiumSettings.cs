using Newtonsoft.Json;

namespace TheInternet.Common.ExecutionContext.Runtime.BrowserSettings
{
    public class AppiumSettings : BrowserSettingsBase
    {
        [JsonProperty("options")]
        public AppiumParameterOptions Options { get; set; }
    }

    public class AppiumParameterOptions
    {
        public AppiumParameterOptions()
        {
        }


    }
}
