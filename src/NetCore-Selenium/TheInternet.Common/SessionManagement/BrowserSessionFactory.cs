using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Events;
using System;
using TheInternet.Common.ExecutionContext.Runtime.BrowserSettings;
using TheInternet.Common.ExecutionContext.Runtime.BrowserSettings.Contracts;
using TheInternet.Common.ExecutionContext.Runtime.RemoteWebDriverSettings;
using TheInternet.Common.SessionManagement.Contracts;

namespace TheInternet.Common.SessionManagement
{
    public class BrowserSessionFactory : IBrowserSessionFactory
    {
        public IBrowserSession Create(IBrowserProperties browserProperties, RemoteWebDriverSettings remoteWebDriverSettings)
        {
            if (browserProperties == null) throw new System.ArgumentNullException(nameof(browserProperties));
            if (remoteWebDriverSettings == null) throw new System.ArgumentNullException(nameof(remoteWebDriverSettings));

            var browser = default(EventFiringWebDriver);

            switch (browserProperties.Name)
            {
                case "CHROME":
                    var browserSettings = browserProperties.BrowserSettings as ChromeBrowserSettings;
                    if (null == browserSettings) throw new System.InvalidOperationException($"The browserSettings for {browserProperties.Name} are not availble. Were they correctly registered in the Container? ");

                    browser = StartBrowser(remoteWebDriverSettings, browserSettings);

                    break;
                default:
                    throw new System.ArgumentOutOfRangeException($"There is no support for starting browsers of type {browserProperties.Name}");
            }

            return new BrowserSession(browser);
        }

        private EventFiringWebDriver StartBrowser(RemoteWebDriverSettings remoteWebDriverSettings, ChromeBrowserSettings settings)
        {
            var adapter = new ChromeBrowserSettingsAdapter();
            var chromeOptions = adapter.ToChromeOptions(settings);

            System.Environment.SetEnvironmentVariable("CHROME_LOG_FILE", null);
            if (settings.Options.CaptureChromeLogFile)
            {
                System.Environment.SetEnvironmentVariable("CHROME_LOG_FILE", settings.Options.ChromeLogPath);
            }
            if (!remoteWebDriverSettings.UseRemoteDriver)
            {
                return new EventFiringWebDriver(new ChromeDriver(chromeOptions));
            }

            return StartRemoteBrowser(remoteWebDriverSettings, chromeOptions);
        }

        private static EventFiringWebDriver StartRemoteBrowser(RemoteWebDriverSettings remoteDriverSettings, DriverOptions options)
        {
            return new EventFiringWebDriver(new RemoteWebDriver(new Uri(remoteDriverSettings.RemoteUri), options));
        }
    }
}
