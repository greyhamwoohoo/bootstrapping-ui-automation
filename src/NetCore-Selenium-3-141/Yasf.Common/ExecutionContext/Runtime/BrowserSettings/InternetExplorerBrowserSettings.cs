using OpenQA.Selenium.IE;

namespace Yasf.Common.ExecutionContext.Runtime.BrowserSettings
{
    public class InternetExplorerBrowserSettings : BrowserSettingsBase
    {
        public string BrowserVersion { get; set; }
        public BrowserSettingsInternetExplorerOptions Options { get; set; }

        public InternetExplorerBrowserSettings()
        {
        }

        public void Cleanse()
        {
            if (BrowserVersion == null) BrowserVersion = "";
            if (Options == null) Options = new BrowserSettingsInternetExplorerOptions();

            Options.Cleanse();
        }

        public class BrowserSettingsInternetExplorerOptions
        {
            public bool AcceptInsecureCerts { get; set; }
            public string BrowserCommandLineArguments { get; set; }
            public bool ForceShellWindowApi { get; set; }
            public bool ForceCreateProcessApi { get; set; }
            public bool EnablePersistentHover { get; set; }
            public InternetExplorerElementScrollBehavior ElementScrollBehavior { get; set; }
            public string InitialBrowserUrl { get; set; }
            public bool RequireWindowFocus { get; set; }
            public bool EnableNativeEvents { get; set; }
            public bool IgnoreZoomLevel { get; set; }
            public bool IntroduceInstabilityByIgnoringProtectedModeSettings { get; set; }
            public bool UsePerProcessProxy { get; set; }
            public bool EnsureCleanSession { get; set; }

            public void Cleanse()
            {
                BrowserCommandLineArguments = BrowserCommandLineArguments ?? "";
            }
        }
    }
}
