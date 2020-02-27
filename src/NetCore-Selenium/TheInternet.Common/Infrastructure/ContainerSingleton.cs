using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using TheInternet.Common.ExecutionContext.Runtime;
using TheInternet.Common.ExecutionContext.Runtime.BrowserSettings;
using TheInternet.Common.ExecutionContext.Runtime.BrowserSettings.Contracts;

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
            if (_instance != null) throw new InvalidOperationException($"The Initialize method has already been called. ");

            var services = new ServiceCollection();

            ConfigureBrowserSettings(prefix, services);

            _instance = services.BuildServiceProvider();
        }

        private static void ConfigureBrowserSettings(string prefix, IServiceCollection services)
        {
            var runtimeSettingsUtilities = new RuntimeSettingsUtilities();
            var paths = runtimeSettingsUtilities.GetSettingsFiles(prefix, Path.Combine(Directory.GetCurrentDirectory(), "Runtime"), "BrowserSettings", "default-chrome.json");
            var configurationRoot = runtimeSettingsUtilities.Buildconfiguration(prefix, paths);

            var browserName = configurationRoot.GetSection("browserName")?.Value?.ToUpper();
            var browserSettings = configurationRoot.GetSection("browserSettings");
            switch(browserName)
            {
                case "CHROME":
                    ChromeBrowserSettings instance = new ChromeBrowserSettings();
                    browserSettings.Bind(instance);
                    instance.BrowserName = browserName;
                    instance.Cleanse();
                    services.AddSingleton(instance);
                    services.AddSingleton<IBrowserName>(instance);
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException($"The browser called {browserName} is currently not supported. ");
            }
        }
    }
}
