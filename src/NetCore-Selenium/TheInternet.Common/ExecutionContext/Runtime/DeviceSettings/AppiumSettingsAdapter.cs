using System;
using System.Collections.Generic;
using System.Linq;

namespace TheInternet.Common.ExecutionContext.Runtime.DeviceSettings
{
    public class AppiumSettingsAdapter
    {
        private Dictionary<string, string> RecognizedGeneralCapabilities = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "app", "string" },
            { "automationName", "string" },
            { "autoWebView", "boolean" },
            { "deviceName", "string" },
            { "fullReset", "boolean" },
            { "newCommandTimeout", "integer" },
            { "noReset", "boolean" },
            { "orientation", "string" },
            { "platformName", "string" },
            { "platformVersion", "string" },
            { "udid", "string" },
            { "eventTimings", "boolean" },
            { "enablePerformanceLogging", "boolean" },
            { "printPageSourceOnFindFailure", "boolean" },
            { "clearSystemFiles", "boolean" },
            { "otherApps", "string" },
            { "browserName", "string" },
            { "language", "string" },
            { "locale", "string" }
        };

        public OpenQA.Selenium.Appium.AppiumOptions ToAppiumOptions(AppiumSettings settings)
        {
            if (settings == null) throw new System.ArgumentNullException(nameof(settings));

            var result = new OpenQA.Selenium.Appium.AppiumOptions();

            // General Capabilities
            PopulateCapabilities(inOptions: result, withWellKnownCapabilities: RecognizedGeneralCapabilities, from: settings?.DeviceSettings?.Options?.GeneralCapabilities);

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

        private void PopulateCapabilities(OpenQA.Selenium.Appium.AppiumOptions inOptions, IDictionary<string, string> withWellKnownCapabilities, IDictionary<string, object> from)
        {
            if (null == withWellKnownCapabilities) throw new System.ArgumentNullException(nameof(withWellKnownCapabilities));
            if (null == inOptions) throw new System.ArgumentNullException(nameof(inOptions));
            
            if (null == from) return;

            from.ToList().ForEach(pair =>
            {
                if (pair.Key.StartsWith("!")) return;

                // NOTE: Environment Variable overrides will populate AppiumSettings with the case of the environment variable. 
                //       ie: ...__APP will set the APP capability (it needs to be 'app')
                var candidateKey = pair.Key;

                var keyIsKnown = RecognizedGeneralCapabilities.ContainsKey(candidateKey);
                if (keyIsKnown)
                {
                    candidateKey = RecognizedGeneralCapabilities.Keys.First(k => string.Compare(k, pair.Key, StringComparison.InvariantCultureIgnoreCase) == 0);
                }

                if (!keyIsKnown)
                {
                    // As the key is not known - do not coerce it. 
                    inOptions.AddAdditionalCapability(candidateKey, pair.Value);
                }
                else
                {
                    inOptions.AddAdditionalCapability(candidateKey, CoerceValueToType(candidateKey, RecognizedGeneralCapabilities[candidateKey], pair.Value));
                }
            });
        }

        private object CoerceValueToType(string candidateKey, string type, object candidateValue)
        {
            var result = candidateValue;

            switch(type.ToLower())
            {
                case "string":
                    break;
                case "boolean":
                    result = System.Convert.ToBoolean(candidateValue);
                    break;
                case "integer":
                    result = System.Convert.ToInt32(candidateValue);
                    break;

                default:
                    break;
            }

            return result;
        }
    }
}
