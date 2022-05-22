using OpenQA.Selenium.Remote;
using Serilog;
using Yasf.Common.ExecutionContext.Runtime.BrowserSettings.Contracts;
using Yasf.Common.ExecutionContext.Runtime.ControlSettings;
using Yasf.Common.ExecutionContext.Runtime.DeviceSettings.Contracts;
using Yasf.Common.ExecutionContext.Runtime.EnvironmentSettings;
using Yasf.Common.ExecutionContext.Runtime.RemoteWebDriverSettings;
using Yasf.Common.Reporting.Contracts;

namespace Yasf.Common.SessionManagement.Contracts
{
    public interface IDriverSessionFactory
    {
        // TODO: Refactor - too many params
        IDriverSession Create(IDeviceProperties deviceProperties, IBrowserProperties properties, RemoteWebDriverSettings remoteWebDriverSettings, EnvironmentSettings environmentSettings, IControlSettings controlSettings, ILogger logger, ITestCaseReporter testCaseReporter, ICommandExecutor httpCommandExecutor);
    }
}
