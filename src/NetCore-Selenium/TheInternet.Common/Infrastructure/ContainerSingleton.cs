using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.Linq;
using TheInternet.Common.ExecutionContext.Contracts;
using TheInternet.Common.ExecutionContext.Runtime;
using TheInternet.Common.ExecutionContext.Runtime.BrowserSettings;
using TheInternet.Common.ExecutionContext.Runtime.BrowserSettings.Contracts;
using TheInternet.Common.ExecutionContext.Runtime.ControlSettings;
using TheInternet.Common.ExecutionContext.Runtime.DeviceSettings;
using TheInternet.Common.ExecutionContext.Runtime.DeviceSettings.Contracts;
using TheInternet.Common.ExecutionContext.Runtime.InstrumentationSettings;
using TheInternet.Common.ExecutionContext.Runtime.RemoteWebDriverSettings;
using TheInternet.Common.Reporting;
using TheInternet.Common.Reporting.Contracts;
using TheInternet.Common.SessionManagement;
using TheInternet.Common.SessionManagement.Contracts;

namespace TheInternet.Common.Infrastructure
{
    public static class ContainerSingleton
    {
        private static IServiceProvider _instance;
        private static object locker = new object();

        public static IServiceProvider Instance
        {
            get
            {
                lock (locker)
                {
                    if (_instance == null) throw new InvalidOperationException($"You must call Container.Initialize exactly once to initialize the container. ");

                    return _instance;
                }
            }
        }
        public static void Initialize(string prefix)
        {
            Initialize(prefix, (prefix, services) => { });
        }

        public static void Initialize(string prefix, Action<string, IServiceCollection> beforeContainerBuild)
        {
            if (_instance != null) throw new InvalidOperationException($"The Initialize method has already been called. ");
            if (prefix == null) throw new System.ArgumentNullException(nameof(prefix));
            if (beforeContainerBuild == null) throw new System.ArgumentNullException(nameof(beforeContainerBuild));

            Environment.SetEnvironmentVariable("TEST_OUTPUT_FOLDER", Directory.GetCurrentDirectory(), EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("TESTDEPLOYMENTDIR", Directory.GetCurrentDirectory(), EnvironmentVariableTarget.Process);

            var services = new ServiceCollection();

            ConfigureDeviceSettings(Log.Logger, prefix, services);
            ConfigureBrowserSettings(Log.Logger, prefix, services);

            ConfigureSettings<RemoteWebDriverSettings>(Log.Logger, prefix, "RemoteWebDriverSettings", "localhost-selenium.json", "remoteWebDriverSettings", services, registerInstance: true);
            ConfigureSettings<EnvironmentSettings>(Log.Logger, prefix, "EnvironmentSettings", "internet.json", "environmentSettings", services, registerInstance: true);
            ConfigureSettings<IInstrumentationSettings, InstrumentationSettings>(Log.Logger, prefix, "InstrumentationSettings", "default.json", "instrumentationSettings", services);
            ConfigureSettings<IControlSettings, ControlSettings>(Log.Logger, prefix, "ControlSettings", "default.json", "controlSettings", services);

            services.AddSingleton<IDriverSessionFactory, DriverSessionFactory>();
            services.AddSingleton<ITestRunReporter>(isp =>
            {
                return new TestRunReporter(isp.GetRequiredService<ILogger>(), testDeploymentFolder: Directory.GetCurrentDirectory());
            });
            services.AddScoped<ITestCaseReporter>(isp =>
            {
                // NOTE: We have no TestContext here - so we rely on this being initialized as early as possible in the test. 
                return new TestCaseReporter(isp.GetRequiredService<ILogger>());
            });
            services.AddScoped(sp =>
            {
                var serilogContext = BuildSerilogConfiguration();

                ILogger logger = new LoggerConfiguration()
                    .ReadFrom
                    .Configuration(serilogContext)
                    .Enrich
                    .FromLogContext()
                    .CreateLogger();

                return logger;
            });

            services.AddScoped(isp =>
            {
                var factory = isp.GetRequiredService<IDriverSessionFactory>();
                var browserProperties = isp.GetRequiredService<IBrowserProperties>();
                var remoteWebDriverSettings = isp.GetRequiredService<RemoteWebDriverSettings>();
                var environmentSettings = isp.GetRequiredService<EnvironmentSettings>();
                var controlSettings = isp.GetRequiredService<IControlSettings>();
                var deviceSettings = isp.GetRequiredService<IDeviceProperties>();
                var logger = isp.GetRequiredService<ILogger>();
                var testCaseReporter = isp.GetRequiredService<ITestCaseReporter>();

                var driverSession = factory.Create(deviceSettings, browserProperties, remoteWebDriverSettings, environmentSettings, controlSettings, logger, testCaseReporter);
                return driverSession;
            });

            beforeContainerBuild(prefix, services);

            _instance = services.BuildServiceProvider();
        }

        private static void ConfigureDeviceSettings(ILogger logger, string prefix, IServiceCollection services)
        {
            var runtimeSettingsUtilities = new RuntimeSettingsUtilities(logger);
            var paths = runtimeSettingsUtilities.GetSettingsFiles(prefix, Path.Combine(Directory.GetCurrentDirectory(), "Runtime"), "DeviceSettings", "desktop-selenium-default.json");
            var configurationRoot = runtimeSettingsUtilities.Buildconfiguration(prefix, paths);

            var platformName = configurationRoot.GetSection("platformName")?.Value?.ToUpper();

            switch(platformName)
            {
                case "DESKTOP":
                    var instance = new DesktopSettings();

                    instance.PlatformName = platformName;

                    services.AddSingleton(instance);
                    services.AddSingleton<IDeviceProperties>(instance);
                    break;
                case "ANDROID":
                    var androidSettings = new AppiumSettings();

                    configurationRoot.Bind(androidSettings);

                    androidSettings = SubstituteEnvironmentVariables<AppiumSettings>(androidSettings);

                    androidSettings.PlatformName = platformName;
                    androidSettings.Cleanse();

                    services.AddSingleton(androidSettings);
                    services.AddSingleton<IDeviceProperties>(androidSettings);
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException($"The device called {platformName} is currently not supported. ");
            }
        }

        private static void ConfigureBrowserSettings(ILogger logger, string prefix, IServiceCollection services)
        {
            var runtimeSettingsUtilities = new RuntimeSettingsUtilities(logger);
            var paths = runtimeSettingsUtilities.GetSettingsFiles(prefix, Path.Combine(Directory.GetCurrentDirectory(), "Runtime"), "BrowserSettings", "default-chrome.json");
            var configurationRoot = runtimeSettingsUtilities.Buildconfiguration(prefix, paths);

            var browserName = configurationRoot.GetSection("browserName")?.Value?.ToUpper();
            var browserSettings = configurationRoot.GetSection("browserSettings");

            switch(browserName)
            {
                case "CHROME":
                    var instance = new ChromeBrowserSettings();
                    
                    browserSettings.Bind(instance);

                    instance = SubstituteEnvironmentVariables<ChromeBrowserSettings>(instance);

                    instance.BrowserName = browserName;
                    instance.Cleanse();

                    services.AddSingleton(instance);
                    services.AddSingleton<IBrowserProperties>(instance);
                    break;
                case "EDGE":
                    var edgeInstance = new EdgeBrowserSettings();

                    browserSettings.Bind(edgeInstance);

                    edgeInstance = SubstituteEnvironmentVariables<EdgeBrowserSettings>(edgeInstance);

                    edgeInstance.BrowserName = browserName;
                    edgeInstance.Cleanse();

                    services.AddSingleton(edgeInstance);
                    services.AddSingleton<IBrowserProperties>(edgeInstance);
                    break;
                case "FIREFOX":
                    var ffInstance = new FireFoxBrowserSettings();

                    browserSettings.Bind(ffInstance);

                    ffInstance = SubstituteEnvironmentVariables<FireFoxBrowserSettings>(ffInstance);

                    ffInstance.BrowserName = browserName;
                    ffInstance.Cleanse();

                    services.AddSingleton(ffInstance);
                    services.AddSingleton<IBrowserProperties>(ffInstance);
                    break;
                case "INTERNETEXPLORER":
                    var ieInstance = new InternetExplorerBrowserSettings();

                    browserSettings.Bind(ieInstance);

                    ieInstance = SubstituteEnvironmentVariables<InternetExplorerBrowserSettings>(ieInstance);

                    ieInstance.BrowserName = browserName;
                    ieInstance.Cleanse();

                    services.AddSingleton(ieInstance);
                    services.AddSingleton<IBrowserProperties>(ieInstance);
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException($"The browser called {browserName} is currently not supported. ");
            }
        }

        private static T ConfigureSettings<T>(ILogger logger, string prefix, string settingsFolderName, string defaultFilename, string settingsName, IServiceCollection services, bool registerInstance = false) where T : class, new()
        {
            var runtimeSettingsUtilities = new RuntimeSettingsUtilities(logger);
            var paths = runtimeSettingsUtilities.GetSettingsFiles(prefix, Path.Combine(Directory.GetCurrentDirectory(), "Runtime"), settingsFolderName, defaultFilename);
            var configurationRoot = runtimeSettingsUtilities.Buildconfiguration(prefix, paths);

            var controlSettings = configurationRoot.GetSection(settingsName);

            var instance = new T();

            controlSettings.Bind(instance);

            instance = SubstituteEnvironmentVariables<T>(instance);

            var cleansor = instance as ICleanse;
            if (cleansor != null)
            {
                cleansor.Cleanse();
            }

            if(registerInstance)
            {
                services.AddSingleton<T>(isp => instance);
            }

            return instance;
        }

        private static void ConfigureSettings<TI, T>(ILogger logger, string prefix, string settingsFolderName, string defaultFilename, string settingsName, IServiceCollection services) where T : class, TI, new() where TI : class
        {
            var instance = ConfigureSettings<T>(logger, prefix, settingsFolderName, defaultFilename, settingsFolderName, services, registerInstance: false);

            services.AddSingleton<TI, T>(isp => instance);
        }

        /// <summary>
        /// Brute force any environment variables embedded within the properties of the object. 
        /// TODO: use a ConfigurationProvider to do this
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private static T SubstituteEnvironmentVariables<T>(T value) where T: class
        {
            var variables = System.Environment.GetEnvironmentVariables();
            var content = JsonConvert.SerializeObject(value);
            if (content.Contains("%"))
            {
                foreach (var variable in variables.Keys.Cast<string>())
                {
                    var candidateString = JsonConvert.ToString(variables[variable]).Trim('"');
                    content = content.Replace($"%{variable.ToUpper()}%", candidateString);
                }

                return JsonConvert.DeserializeObject<T>(content);
            } 
            else
            {
                return value;    
            }
        }

        private static IConfigurationRoot BuildSerilogConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("serilogsettings.json", optional: false)
                .Build();

            return configuration;
        }
    }
}
