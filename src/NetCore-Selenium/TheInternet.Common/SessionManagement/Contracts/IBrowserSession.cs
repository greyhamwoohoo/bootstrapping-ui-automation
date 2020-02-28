using OpenQA.Selenium;
using TheInternet.Common.ExecutionContext.Runtime.RemoteWebDriverSettings;

namespace TheInternet.Common.SessionManagement.Contracts
{
    public interface IBrowserSession
    {
        IWebDriver WebDriver { get; }
        EnvironmentSettings EnvironmentSettings { get; }
    }
}
