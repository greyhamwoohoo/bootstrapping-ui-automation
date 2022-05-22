using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using Serilog;
using System;
using System.Linq;
using System.Text;
using Yasf.Common.Drivers;
using Yasf.Common.ElementOperations;
using Yasf.Common.ExecutionContext.Runtime.BrowserSettings;
using Yasf.Common.ExecutionContext.Runtime.BrowserSettings.Contracts;
using Yasf.Common.ExecutionContext.Runtime.ControlSettings;
using Yasf.Common.ExecutionContext.Runtime.DeviceSettings;
using Yasf.Common.ExecutionContext.Runtime.DeviceSettings.Contracts;
using Yasf.Common.ExecutionContext.Runtime.EnvironmentSettings;
using Yasf.Common.ExecutionContext.Runtime.RemoteWebDriverSettings;
using Yasf.Common.Reporting.Contracts;
using Yasf.Common.SessionManagement.Contracts;

namespace Yasf.Common.SessionManagement
{
    public class DriverSessionFactory : IDriverSessionFactory
    {
        // TODO: Refactor. Too many parameters. 
        public IDriverSession Create(IDeviceProperties deviceProperties, IBrowserProperties browserProperties, RemoteWebDriverSettings remoteWebDriverSettings, EnvironmentSettings environmentSettings, IControlSettings controlSettings, ILogger logger, ITestCaseReporter testCaseReporter, ICommandExecutor httpCommandExecutor)
        {
            if (deviceProperties == null) throw new ArgumentNullException(nameof(deviceProperties));
            if (browserProperties == null) throw new ArgumentNullException(nameof(browserProperties));
            if (remoteWebDriverSettings == null) throw new ArgumentNullException(nameof(remoteWebDriverSettings));
            if (environmentSettings == null) throw new ArgumentNullException(nameof(environmentSettings));
            if (controlSettings == null) throw new ArgumentNullException(nameof(controlSettings));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (testCaseReporter == null) throw new ArgumentNullException(nameof(testCaseReporter));
            if (httpCommandExecutor == null) throw new ArgumentNullException(nameof(httpCommandExecutor));

            var browser = default(IWebDriver);

            // There are several kinds of platform we can create our software on:
            // 1. Desktop (ie: 'traditional' Web Browser Automation)
            // 2. Android (Devices)

            if (deviceProperties.Name == "DESKTOP")
            {
                switch (browserProperties.Name)
                {
                    case "CHROME":
                        var chromeBrowserSettings = browserProperties.BrowserSettings as ChromeBrowserSettings;
                        if (null == chromeBrowserSettings) throw new InvalidOperationException($"The browserSettings for {browserProperties.Name} are not availble. Were they correctly registered in the Container? ");

                        browser = StartBrowser(remoteWebDriverSettings, chromeBrowserSettings, controlSettings);

                        break;
                    case "EDGE":
                        var edgeBrowserSettings = browserProperties.BrowserSettings as EdgeBrowserSettings;
                        if (null == edgeBrowserSettings) throw new InvalidOperationException($"The browserSettings for {browserProperties.Name} are not availble. Were they correctly registered in the Container? ");

                        browser = StartBrowser(remoteWebDriverSettings, edgeBrowserSettings, controlSettings);

                        break;
                    case "INTERNETEXPLORER":
                        var ieBrowserSettings = browserProperties.BrowserSettings as InternetExplorerBrowserSettings;
                        if (null == ieBrowserSettings) throw new InvalidOperationException($"The browserSettings for {browserProperties.Name} are not availble. Were they correctly registered in the Container? ");

                        browser = StartBrowser(remoteWebDriverSettings, ieBrowserSettings, controlSettings);

                        break;
                    case "FIREFOX":
                        var ffBrowserSettings = browserProperties.BrowserSettings as FireFoxBrowserSettings;
                        if (null == ffBrowserSettings) throw new InvalidOperationException($"The browserSettings for {browserProperties.Name} are not availble. Were they correctly registered in the Container? ");

                        browser = StartBrowser(remoteWebDriverSettings, ffBrowserSettings, controlSettings);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"There is no support for starting browsers of type {browserProperties.Name}");
                }

                var decoratedWebDriver = new DecoratedWebDriver(browser, controlSettings, testCaseReporter, logger);

                return new DriverSession(decoratedWebDriver, environmentSettings, controlSettings, testCaseReporter);
            }

            if (deviceProperties.Name == "ANDROID")
            {
                if (!remoteWebDriverSettings.UseRemoteDriver)
                {
                    throw new InvalidOperationException($"For Appium, we always configure the RemoteWebDriver settings. If running Appium locally, use the 'common-localhost-appium.json' file. ");
                }

                var appiumSettings = deviceProperties.DeviceSettings as AppiumSettings;
                var appiumSettingsAdapter = new AppiumSettingsAdapter();
                var options = appiumSettingsAdapter.ToAppiumOptions(appiumSettings);

                var androidDriver = default(AndroidDriver);

                foreach (var currentAttempt in Enumerable.Range(1, controlSettings.AppiumDriverCreationRetries))
                {
                    try
                    {
                        androidDriver = new AndroidDriver(httpCommandExecutor, options);

                        break;
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"ERROR: Starting Android Driver attempt {currentAttempt}");
                        logger.Error($"ERROR: {ex}");

                        if (currentAttempt == controlSettings.AppiumDriverCreationRetries)
                        {
                            throw;
                        }
                    }
                }

                return new DriverSession(new DecoratedWebDriver(androidDriver, controlSettings, testCaseReporter, logger), environmentSettings, controlSettings, testCaseReporter);
            }

            throw new InvalidOperationException($"The device {deviceProperties.Name} is not supported as a Driver Session. ");
        }

        private IWebDriver StartBrowser(RemoteWebDriverSettings remoteWebDriverSettings, ChromeBrowserSettings settings, IControlSettings controlSettings)
        {
            var adapter = new ChromeBrowserSettingsAdapter();
            var chromeOptions = adapter.ToChromeOptions(settings);

            Environment.SetEnvironmentVariable("CHROME_LOG_FILE", null);
            if (settings.Options.CaptureChromeLogFile)
            {
                Environment.SetEnvironmentVariable("CHROME_LOG_FILE", settings.Options.ChromeLogPath);
            }
            if (!remoteWebDriverSettings.UseRemoteDriver)
            {
                var chromeDriver = controlSettings.AttachToExistingSessionIfItExists ? new AttachableChromeDriver(chromeOptions) : new ChromeDriver(chromeOptions);
                return chromeDriver;
            }

            return StartRemoteBrowser(remoteWebDriverSettings, chromeOptions, controlSettings);
        }

        private static IWebDriver StartBrowser(RemoteWebDriverSettings remoteDriverSettings, FireFoxBrowserSettings settings, IControlSettings controlSettings)
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
                return new FirefoxDriver(options);
            }

            return StartRemoteBrowser(remoteDriverSettings, options, controlSettings);
        }

        private static IWebDriver StartBrowser(RemoteWebDriverSettings remoteDriverSettings, InternetExplorerBrowserSettings settings, IControlSettings controlSettings)
        {
            var adapter = new InternetExplorerBrowserSettingsAdapter();
            InternetExplorerOptions options = adapter.ToInternetExplorerOptions(settings);

            if (!remoteDriverSettings.UseRemoteDriver)
            {
                var ieDriver = controlSettings.AttachToExistingSessionIfItExists ? new AttachableInternetExplorerDriver(options) : new InternetExplorerDriver(options);

                return ieDriver;
            }

            return StartRemoteBrowser(remoteDriverSettings, options, controlSettings);
        }

        private static IWebDriver StartBrowser(RemoteWebDriverSettings remoteDriverSettings, EdgeBrowserSettings settings, IControlSettings controlSettings)
        {
            var adapter = new EdgeBrowserSettingsAdapter();
            var options = adapter.ToEdgeOptions(settings);

            if (!remoteDriverSettings.UseRemoteDriver)
            {
                var edgeDriver = controlSettings.AttachToExistingSessionIfItExists ? new AttachableEdgeDriver(options) : new EdgeDriver(options);

                return edgeDriver;
            }

            return StartRemoteBrowser(remoteDriverSettings, options, controlSettings);
        }

        private static IWebDriver StartRemoteBrowser(RemoteWebDriverSettings remoteDriverSettings, DriverOptions options, IControlSettings controlSettings)
        {
            RemoteWebDriver remoteWebDriver = controlSettings.AttachToExistingSessionIfItExists ? new AttachableRemoteWebDriver(new Uri(remoteDriverSettings.RemoteUri), options) : new RemoteWebDriver(new Uri(remoteDriverSettings.RemoteUri), options);
            return remoteWebDriver;
        }
    }
}
