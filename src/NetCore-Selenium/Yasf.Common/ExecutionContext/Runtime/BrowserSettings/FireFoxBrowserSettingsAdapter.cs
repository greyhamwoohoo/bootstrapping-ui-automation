using OpenQA.Selenium.Firefox;

namespace Yasf.Common.ExecutionContext.Runtime.BrowserSettings
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
                bool isBool = bool.TryParse(System.Convert.ToString(pair.Value), out boolValue);
                if (isBool)
                {
                    result.SetPreference(pair.Key, boolValue);
                    continue;
                }

                int intValue;
                bool isInt = int.TryParse(System.Convert.ToString(pair.Value), out intValue);
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
            return result;
        }
    }
}
