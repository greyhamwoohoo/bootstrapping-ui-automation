using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Events;
using System;
using System.Text;
using TheInternet.Common.ExecutionContext.Runtime.BrowserSettings;
using TheInternet.Common.ExecutionContext.Runtime.BrowserSettings.Contracts;
using TheInternet.Common.ExecutionContext.Runtime.RemoteWebDriverSettings;
using TheInternet.Common.SessionManagement.Contracts;

namespace TheInternet.Common.SessionManagement
{
    public class BrowserSessionFactory : IBrowserSessionFactory
    {
        public IBrowserSession Create(IBrowserProperties browserProperties, RemoteWebDriverSettings remoteWebDriverSettings, EnvironmentSettings environmentSettings)
        {
            if (browserProperties == null) throw new System.ArgumentNullException(nameof(browserProperties));
            if (remoteWebDriverSettings == null) throw new System.ArgumentNullException(nameof(remoteWebDriverSettings));
            if (environmentSettings == null) throw new System.ArgumentNullException(nameof(environmentSettings));

            var browser = default(EventFiringWebDriver);

            switch (browserProperties.Name)
            {
                case "CHROME":
                    var chromeBrowserSettings = browserProperties.BrowserSettings as ChromeBrowserSettings;
                    if (null == chromeBrowserSettings) throw new System.InvalidOperationException($"The browserSettings for {browserProperties.Name} are not availble. Were they correctly registered in the Container? ");

                    browser = StartBrowser(remoteWebDriverSettings, chromeBrowserSettings);

                    break;
                case "EDGE":
                    var edgeBrowserSettings = browserProperties.BrowserSettings as EdgeBrowserSettings;
                    if (null == edgeBrowserSettings) throw new System.InvalidOperationException($"The browserSettings for {browserProperties.Name} are not availble. Were they correctly registered in the Container? ");

                    browser = StartBrowser(remoteWebDriverSettings, edgeBrowserSettings);

                    break;
                case "FIREFOX":
                    var ffBrowserSettings = browserProperties.BrowserSettings as FireFoxBrowserSettings;
                    if (null == ffBrowserSettings) throw new System.InvalidOperationException($"The browserSettings for {browserProperties.Name} are not availble. Were they correctly registered in the Container? ");

                    browser = StartBrowser(remoteWebDriverSettings, ffBrowserSettings);

                    break;
                default:
                    throw new System.ArgumentOutOfRangeException($"There is no support for starting browsers of type {browserProperties.Name}");
            }

            return new BrowserSession(browser, environmentSettings);
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
        private static EventFiringWebDriver StartBrowser(RemoteWebDriverSettings remoteDriverSettings, FireFoxBrowserSettings settings)
        {
            // Reference: https://stackoverflow.com/questions/41644381/python-set-firefox-preferences-for-selenium-download-location
            var adapter = new FireFoxBrowserSettingsAdapter();
            var profile = adapter.ToFirefoxProfile(settings);
            var options = adapter.ToFirefoxOptions(settings);

            // Reference: In .Net Core, we need to explicitly set the encoding
            // https://github.com/SeleniumHQ/selenium/issues/4816
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            options.Profile = profile;

            if (!remoteDriverSettings.UseRemoteDriver)
            {
                return new EventFiringWebDriver(new FirefoxDriver(options));
            }

            return StartRemoteBrowser(remoteDriverSettings, options);
        }

        private static EventFiringWebDriver StartBrowser(RemoteWebDriverSettings remoteDriverSettings, EdgeBrowserSettings settings)
        {
            var adapter = new EdgeBrowserSettingsAdapter();
            var options = adapter.ToEdgeOptions(settings);

            if (!remoteDriverSettings.UseRemoteDriver)
            {
                return new EventFiringWebDriver(new EdgeDriver(options));
            }

            return StartRemoteBrowser(remoteDriverSettings, options);
        }

        private static EventFiringWebDriver StartRemoteBrowser(RemoteWebDriverSettings remoteDriverSettings, DriverOptions options)
        {
            return new EventFiringWebDriver(new RemoteWebDriver(new Uri(remoteDriverSettings.RemoteUri), options));
        }
    }
}
