using OpenQA.Selenium;
using TheInternet.Common.ElementOperations;
using TheInternet.Common.ElementOperations.Contracts;
using TheInternet.Common.ExecutionContext.Runtime.ControlSettings;
using TheInternet.Common.ExecutionContext.Runtime.RemoteWebDriverSettings;
using TheInternet.Common.Reporting.Contracts;

namespace TheInternet.Common.SessionManagement.Contracts
{
    public interface IDriverSession
    {
        EnvironmentSettings EnvironmentSettings { get; }
        IControlSettings ControlSettings { get; }
        IWaiter Waiter { get; }
        IDecoratedWebDriver WebDriver { get; }
        ITestCaseReporter TestCaseReporter { get; }
    }
}
