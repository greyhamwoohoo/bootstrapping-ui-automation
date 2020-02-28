using OpenQA.Selenium;
using TheInternet.Common.ExecutionContext.Runtime.RemoteWebDriverSettings;
using TheInternet.Common.SessionManagement.Contracts;

namespace TheInternet.Common.SessionManagement
{
    public class BrowserSession : IBrowserSession
    {
        public IWebDriver WebDriver { get; }
        public EnvironmentSettings EnvironmentSettings { get; }

        public BrowserSession(IWebDriver webDriver, EnvironmentSettings environmentSettings) 
        {
            if (webDriver == null) throw new System.ArgumentNullException(nameof(webDriver));
            if (environmentSettings == null) throw new System.ArgumentNullException(nameof(environmentSettings));

            WebDriver = webDriver;
            EnvironmentSettings = environmentSettings;
        }
    }
}
