using OpenQA.Selenium;
using TheInternet.Common.ElementOperations;
using TheInternet.Common.ExecutionContext.Runtime.ControlSettings;
using TheInternet.Common.ExecutionContext.Runtime.RemoteWebDriverSettings;

namespace TheInternet.Common.SessionManagement.Contracts
{
    public interface IBrowserSession
    {
        EnvironmentSettings EnvironmentSettings { get; }
        IControlSettings ControlSettings { get; }
        IWaiter Waiter { get; }
        IWebDriver WebDriver { get; }
    }
}
