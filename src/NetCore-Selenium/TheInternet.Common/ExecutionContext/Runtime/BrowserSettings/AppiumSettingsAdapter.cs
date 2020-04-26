using OpenQA.Selenium.Appium;

namespace TheInternet.Common.ExecutionContext.Runtime.BrowserSettings
{
    public class AppiumSettingsAdapter
    {
        public AppiumOptions ToAppiumOptions(AppiumSettings settings)
        {
            if (settings == null) throw new System.ArgumentNullException(nameof(settings));

            var result = new AppiumOptions();

            return result;
        }
    }
}
