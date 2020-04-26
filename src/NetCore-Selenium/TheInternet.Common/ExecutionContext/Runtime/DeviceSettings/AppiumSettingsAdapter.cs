using System.Linq;

namespace TheInternet.Common.ExecutionContext.Runtime.DeviceSettings
{
    public class AppiumSettingsAdapter
    {
        public OpenQA.Selenium.Appium.AppiumOptions ToAppiumOptions(AppiumSettings settings)
        {
            if (settings == null) throw new System.ArgumentNullException(nameof(settings));

            var result = new OpenQA.Selenium.Appium.AppiumOptions();

            // For some reason: I need to add this first... else I get the appium:BrowserName standard capability error

            // Mobile Capabilities
            if (settings.DeviceSettings?.Options?.MobileCapabilities != null)
            {
                settings.DeviceSettings.Options.MobileCapabilities.ToList().ForEach(pair =>
                {
                    if (pair.Key.StartsWith("!")) return;

                    result.AddAdditionalCapability(pair.Key, pair.Value);
                });
            }

            // Android Capabilities
            if (settings.DeviceSettings?.Options?.AndroidCapabilities != null)
            {
                settings.DeviceSettings.Options.AndroidCapabilities.ToList().ForEach(pair =>
                {
                    if (pair.Key.StartsWith("!")) return;

                    result.AddAdditionalCapability(pair.Key, pair.Value);
                });
            }

            return result;
        }
    }
}
