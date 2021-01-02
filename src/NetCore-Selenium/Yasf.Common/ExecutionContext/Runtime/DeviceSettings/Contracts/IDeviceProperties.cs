namespace TheInternet.Common.ExecutionContext.Runtime.DeviceSettings.Contracts
{
    public interface IDeviceProperties
    {
        string Name { get; }
        DeviceSettingsBase DeviceSettings { get; }
    }
}
