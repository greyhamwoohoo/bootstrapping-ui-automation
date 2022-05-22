using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using System.Linq;

namespace Yasf.Common.ExecutionContext.Runtime.BrowserSettings
{
    public class ChromeBrowserSettingsAdapter
    {
        public ChromeOptions ToChromeOptions(ChromeBrowserSettings settings)
        {
            if (settings == null) throw new System.ArgumentNullException(nameof(settings));

            var result = new ChromeOptions();

            settings.Options.Arguments.ToList().ForEach(argument => result.AddArgument(argument));
            settings.Options.LoggingPreferences.ToList().ForEach(loggingPreference => result.SetLoggingPreference(loggingPreference.Key, loggingPreference.Value));
            settings.Options.UserProfilePreferences.ToList().ForEach(userProfilePreference => result.AddUserProfilePreference(userProfilePreference.Key, userProfilePreference.Value));

            if (!settings.Options.PerformanceLoggingPreferences.IsEmpty)
            {
                // It would appear that setting Options.PerformanceLoggingPreferences to non-null has side effects; hence, we only set it if we actually have tracing categories to deal with. 
                result.PerformanceLoggingPreferences = new ChromiumPerformanceLoggingPreferences();
                settings.Options.PerformanceLoggingPreferences.TracingCategories.ForEach(tracingCategory => result.PerformanceLoggingPreferences.AddTracingCategory(tracingCategory));
            }

            return result;
        }
    }
}
