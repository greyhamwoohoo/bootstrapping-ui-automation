using OpenQA.Selenium.Firefox;

namespace TheInternet.Common.ExecutionContext.Runtime.BrowserSettings
{
    public class FireFoxBrowserSettingsAdapter
    {
        public FirefoxProfile ToFirefoxProfile(FireFoxBrowserSettings settings)
        {
            if (null == settings) throw new System.ArgumentNullException(nameof(settings));
            
            var result = new FirefoxProfile();

            foreach (var pair in settings.Profile.Preferences)
            {
                bool boolValue;
                bool isBool = System.Boolean.TryParse(System.Convert.ToString(pair.Value), out boolValue);
                if (isBool)
                {
                    result.SetPreference(pair.Key, boolValue);
                    continue;
                }

                int intValue;
                bool isInt = System.Int32.TryParse(System.Convert.ToString(pair.Value), out intValue);
                if (isInt)
                {
                    result.SetPreference(pair.Key, intValue);
                    continue;
                }

                result.SetPreference(pair.Key, System.Convert.ToString(pair.Value));
            }

            return result;
        }

        public FirefoxOptions ToFirefoxOptions(FireFoxBrowserSettings settings)
        {
            if (null == settings) throw new System.ArgumentNullException(nameof(settings));

            var result = new FirefoxOptions();
            result.UseLegacyImplementation = settings.Options.UseLegacyImplementation;
            return result;
        }
    }
}
