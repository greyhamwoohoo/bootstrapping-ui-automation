using System.Collections.Generic;
using System.Linq;

namespace Yasf.Common.ExecutionContext.Runtime.BrowserSettings
{
    public class FireFoxBrowserSettings : BrowserSettingsBase
    {
        public FirefoxBrowserSettingsProfile Profile { get; set; }
        public FirefoxBrowserSettingsOptions Options { get; set; }

        public FireFoxBrowserSettings()
        {
            Cleanse();
        }

        public void Cleanse()
        {
            if (Profile == null) Profile = new FirefoxBrowserSettingsProfile();
            if (Options == null) Options = new FirefoxBrowserSettingsOptions();

            Profile.Cleanse();
        }

        public class FirefoxBrowserSettingsProfile
        {
            public Dictionary<string, object> Preferences { get; set; }

            public FirefoxBrowserSettingsProfile()
            {
                Cleanse();
            }

            public void Cleanse()
            {
                if (Preferences == null) Preferences = new Dictionary<string, object>();

                Preferences = new Dictionary<string, object>(Preferences.Where(pair => !pair.Key.StartsWith("!")).ToDictionary(pair => pair.Key, pair => pair.Value));
            }
        }

        public class FirefoxBrowserSettingsOptions
        {
            public bool UseLegacyImplementation { get; set; }

            public FirefoxBrowserSettingsOptions()
            {
            }
        }
    }
}
