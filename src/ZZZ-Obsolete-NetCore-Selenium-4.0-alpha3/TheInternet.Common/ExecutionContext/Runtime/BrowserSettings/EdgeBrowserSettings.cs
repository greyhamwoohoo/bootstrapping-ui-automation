namespace TheInternet.Common.ExecutionContext.Runtime.BrowserSettings
{
    public class EdgeBrowserSettings : BrowserSettingsBase
    {
        public BrowserSettingsEdgeOptions Options { get; set; }

        public EdgeBrowserSettings()
        {
            Cleanse();
        }

        public void Cleanse()
        {
            if (Options == null) Options = new BrowserSettingsEdgeOptions();
        }

        public class BrowserSettingsEdgeOptions
        {
            public bool UseInPrivateBrowsing { get; set; }

            public BrowserSettingsEdgeOptions()
            {
            }
        }
    }
}
