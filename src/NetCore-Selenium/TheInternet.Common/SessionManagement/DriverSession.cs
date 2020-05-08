using OpenQA.Selenium;
using Serilog;
using TheInternet.Common.ElementOperations;
using TheInternet.Common.ElementOperations.Contracts;
using TheInternet.Common.ExecutionContext.Runtime.ControlSettings;
using TheInternet.Common.ExecutionContext.Runtime.RemoteWebDriverSettings;
using TheInternet.Common.SessionManagement.Contracts;

namespace TheInternet.Common.SessionManagement
{
    public class DriverSession : IDriverSession
    {
        public IDecoratedWebDriver WebDriver { get; }
        public EnvironmentSettings EnvironmentSettings { get; }
        public IControlSettings ControlSettings { get; }
        public IWaiter Waiter { get; }

        public DriverSession(IDecoratedWebDriver webDriver, EnvironmentSettings environmentSettings, ILogger logger, IControlSettings controlSettings) 
        {
            if (webDriver == null) throw new System.ArgumentNullException(nameof(webDriver));
            if (environmentSettings == null) throw new System.ArgumentNullException(nameof(environmentSettings));
            if (controlSettings == null) throw new System.ArgumentNullException(nameof(controlSettings));

            WebDriver = webDriver;
            EnvironmentSettings = environmentSettings;
            ControlSettings = controlSettings;
            Waiter = new Waiter(webDriver, logger, controlSettings);
        }
    }
}
