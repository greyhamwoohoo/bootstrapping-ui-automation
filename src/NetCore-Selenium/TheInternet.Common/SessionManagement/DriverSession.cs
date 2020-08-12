using OpenQA.Selenium;
using Serilog;
using TheInternet.Common.ElementOperations;
using TheInternet.Common.ElementOperations.Contracts;
using TheInternet.Common.ExecutionContext.Runtime.ControlSettings;
using TheInternet.Common.ExecutionContext.Runtime.RemoteWebDriverSettings;
using TheInternet.Common.Reporting.Contracts;
using TheInternet.Common.SessionManagement.Contracts;

namespace TheInternet.Common.SessionManagement
{
    public class DriverSession : IDriverSession
    {
        public IDecoratedWebDriver WebDriver { get; }
        public EnvironmentSettings EnvironmentSettings { get; }
        public IControlSettings ControlSettings { get; }
        public ITestCaseReporter TestCaseReporter { get; }

        public DriverSession(IDecoratedWebDriver webDriver, EnvironmentSettings environmentSettings, IControlSettings controlSettings, ITestCaseReporter testCaseReporter) 
        {
            if (webDriver == null) throw new System.ArgumentNullException(nameof(webDriver));
            if (environmentSettings == null) throw new System.ArgumentNullException(nameof(environmentSettings));
            if (controlSettings == null) throw new System.ArgumentNullException(nameof(controlSettings));
            if (testCaseReporter == null) throw new System.ArgumentNullException(nameof(testCaseReporter));

            WebDriver = webDriver;
            EnvironmentSettings = environmentSettings;
            ControlSettings = controlSettings;
            TestCaseReporter = testCaseReporter;
        }
    }
}
