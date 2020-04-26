using Newtonsoft.Json;
using System.Collections.Generic;

namespace TheInternet.Common.ExecutionContext.Runtime.DeviceSettings
{
    public class AppiumSettings : DeviceSettingsBase
    {
        [JsonProperty("deviceSettings")]
        public AppiumDeviceSettings DeviceSettings { get; set; }

        public void Cleanse()
        {
            if (DeviceSettings == null) DeviceSettings = new AppiumDeviceSettings();

            DeviceSettings.Cleanse();
        }
    }

    public class AppiumDeviceSettings
    {
        public AppiumDeviceSettings()
        {
        }

        [JsonProperty("options")]
        public AppiumSettingsOptions Options { get; set; }

        public void Cleanse()
        {
            if (Options == null) Options = new AppiumSettingsOptions();

            Options.Cleanse();
        }
    }

    public class AppiumSettingsOptions
    {
        [JsonProperty("mobileCapabilities")]
        public Dictionary<string, object> MobileCapabilities { get; set; }

        [JsonProperty("androidCapabilities")]
        public Dictionary<string, object> AndroidCapabilities { get; set; }

        public AppiumSettingsOptions()
        {
        }

        public void Cleanse()
        {
            if (MobileCapabilities == null) MobileCapabilities = new Dictionary<string, object>();
            if (AndroidCapabilities == null) AndroidCapabilities = new Dictionary<string, object>();
        }
    }
}
