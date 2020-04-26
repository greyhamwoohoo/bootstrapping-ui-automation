namespace TheInternet.Common.ExecutionContext.Runtime.DeviceSettings
{
    public class AppiumSettingsAdapter
    {
        public OpenQA.Selenium.Appium.AppiumOptions ToAppiumOptions(AppiumSettings settings)
        {
            if (settings == null) throw new System.ArgumentNullException(nameof(settings));

            var result = new OpenQA.Selenium.Appium.AppiumOptions();

            return result;
        }
    }
}
