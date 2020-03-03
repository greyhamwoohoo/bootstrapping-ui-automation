using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;

namespace TheInternet.Common.ExecutionContext.Runtime.BrowserSettings
{
    public class InternetExplorerBrowserSettingsAdapter
    {
        public InternetExplorerOptions ToInternetExplorerOptions(InternetExplorerBrowserSettings settings)
        {
            var result = new InternetExplorerOptions();
            result.AcceptInsecureCertificates = settings.Options.AcceptInsecureCerts;
            result.BrowserCommandLineArguments = settings.Options.BrowserCommandLineArguments;
            result.BrowserVersion = settings.BrowserVersion;
            result.ForceShellWindowsApi = settings.Options.ForceShellWindowApi;
            result.ForceCreateProcessApi = settings.Options.ForceCreateProcessApi;
            result.EnablePersistentHover = settings.Options.EnablePersistentHover;
            result.ElementScrollBehavior = settings.Options.ElementScrollBehavior;
            result.InitialBrowserUrl = settings.Options.InitialBrowserUrl;
            result.RequireWindowFocus = settings.Options.RequireWindowFocus;
            result.EnableNativeEvents = settings.Options.EnableNativeEvents;
            result.IgnoreZoomLevel = settings.Options.IgnoreZoomLevel;
            result.IntroduceInstabilityByIgnoringProtectedModeSettings = settings.Options.IntroduceInstabilityByIgnoringProtectedModeSettings;
            result.UsePerProcessProxy = settings.Options.UsePerProcessProxy;
            result.EnsureCleanSession = settings.Options.EnsureCleanSession;
            return result;
        }
    }
}
