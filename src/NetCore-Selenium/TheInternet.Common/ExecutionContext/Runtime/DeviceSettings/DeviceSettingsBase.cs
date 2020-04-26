using Newtonsoft.Json;
using TheInternet.Common.ExecutionContext.Runtime.DeviceSettings.Contracts;

namespace TheInternet.Common.ExecutionContext.Runtime.DeviceSettings
{
    public class DeviceSettingsBase : IDeviceProperties
    {
        [JsonProperty("deviceName")]
        public string DeviceName { get; set; }
        
        string IDeviceProperties.Name => DeviceName;

        DeviceSettingsBase IDeviceProperties.DeviceSettings => this;
    }
}
