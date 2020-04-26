using Serilog;
using TheInternet.Common.ExecutionContext.Runtime.BrowserSettings.Contracts;
using TheInternet.Common.ExecutionContext.Runtime.ControlSettings;
using TheInternet.Common.ExecutionContext.Runtime.DeviceSettings.Contracts;
using TheInternet.Common.ExecutionContext.Runtime.RemoteWebDriverSettings;

namespace TheInternet.Common.SessionManagement.Contracts
{
    public interface IDriverSessionFactory
    {
        // TODO: Refactor - too many params
        IDriverSession Create(IDeviceProperties deviceProperties, IBrowserProperties properties, RemoteWebDriverSettings remoteWebDriverSettings, EnvironmentSettings environmentSettings, IControlSettings controlSettings, ILogger logger);
    }
}
