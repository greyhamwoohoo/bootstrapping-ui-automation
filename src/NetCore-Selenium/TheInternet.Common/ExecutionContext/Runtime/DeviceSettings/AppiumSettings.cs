using Newtonsoft.Json;

namespace TheInternet.Common.ExecutionContext.Runtime.DeviceSettings
{
    public class AppiumSettings : DeviceSettingsBase
    {
        [JsonProperty("options")]
        public AppiumOptions Options { get; set; }
    }

    public class AppiumOptions
    {
        public AppiumOptions()
        {
        }
    }
}
