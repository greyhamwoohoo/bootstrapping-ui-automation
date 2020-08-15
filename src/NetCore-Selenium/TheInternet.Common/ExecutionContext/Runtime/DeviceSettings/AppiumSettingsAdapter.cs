using System;
using System.Collections.Generic;
using System.Linq;

namespace TheInternet.Common.ExecutionContext.Runtime.DeviceSettings
{
    public class AppiumSettingsAdapter
    {
        // TODO: Farm these out somewhere

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

        private Dictionary<string, string> RecognizedAndroidCapabilities = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "chromedriverExecutable", "string" },
            { "appActivity", "string" },
            { "appPackage", "string" },
            { "appWaitActivity", "string" },
            { "appWaitPackage", "string" },
            { "appWaitDuration", "integer" },
            { "deviceReadyTimeout", "integer" },
            { "allowTestPackages", "boolean" },
            { "androidCoverage", "string" },
            { "androidCoverageEndIntent", "string" },
            { "androidDeviceReadyTimeout", "integer" },
            { "androidInstallTimeout", "integer" },
            { "androidInstallPath", "string" },
            { "adbPort", "integer" },
            { "systemPort", "integer" },
            { "remoteAdbHost", "string" },
            { "androidDeviceSocker", "string" },
            { "avd", "string" },
            { "avdLaunchTimeout", "integer" },
            { "avdReadyTimeout", "integer" },
            { "avdArgs", "string" },
            { "useKeystore", "boolean" },
            { "keystorePath", "string" },
            { "keystorePassword", "string" },
            { "keyAlias", "string" },
            { "keyPassword", "string" },
            { "chromedriverArgs", "string" },
            { "chromedriverExecutableDir", "string" },
            { "chromedriverChromeMappingFile", "string" },
            { "chromedriverUseSystemExecutable", "boolean" },
            { "autoWebviewTimeout", "integer" },
            { "chromedriverPort", "integer" },
            { "chromedriverPorts", "array" },
            { "ensureWebviewsHavePages", "boolean" },
            { "webviewDevtoolsPort", "integer" },
            { "enableWebviewDetailsCollection", "boolean" },
            { "dontStopAppOnReset", "boolean" },
            { "unicodeKeyboard", "boolean" },
            { "resetKeyboard", "boolean" },
            { "noSign", "boolean" },
            { "ignoreUnimportantViews", "boolean" },
            { "disableAndroidWatchers", "boolean" },
            { "chromeOptions", "object" },
            { "recreateChromeDriverSessions", "boolean" },
            { "nativeScreenshot", "boolean" },
            { "androidScreenshotPath", "string" },
            { "autoGrantPermissions", "boolean" },
            { "networkSpeed", "string" },
            { "gpsEnabled", "boolean" },
            { "isHeadless", "boolean" },
            { "adbExecTimeout", "integer" },
            { "localeScript", "string" },
            { "skipDeviceInitialization", "boolean" },
            { "chromedriverDisableBuildCheck", "boolean" },
            { "skipUnlock", "boolean" },
            { "unlockType", "string" },
            { "unlockKey", "string" },
            { "autoLaunch", "boolean" },
            { "skipLogcatCapture", "boolean" },
            { "uninstallOtherPackages", "string" },
            { "disableWindowAnimations", "boolean" },
            { "remoteAppsCacheLimit", "integer" },
            { "buildToolsVersion", "string" },
            { "androidNaturalOrientation", "boolean" },
            { "enforceAppInstall", "boolean" },
            { "ignoreHiddenApiPolicyError", "boolean" },
            { "mockLocationApp", "string" },
            { "logcatFormat", "string" },
            { "logcatFilterSpecs", "array" }
        };

        private Dictionary<string, string> RecognizedUIAutomatorCapabilities = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "intentActions", "string" },
            { "intentCategory", "string" },
            { "intentFlags", "integer" },
            { "optionalIntentArguments", "string" },
            { "appWaitForLaunch", "boolean" },
            { "disableSuppressAccessibilityService", "string" },
            { "mjpegServerPort", "integer" },
            { "skipServerInstallation", "boolean" },
            { "uiautomator2ServerInstallTimeout", "integer" },
            { "uiautomator2ServerLaunchTimeout", "integer" },
            { "userProfile", "integer" }
        };

        public OpenQA.Selenium.Appium.AppiumOptions ToAppiumOptions(AppiumSettings settings)
        {
            if (settings == null) throw new System.ArgumentNullException(nameof(settings));

            var result = new OpenQA.Selenium.Appium.AppiumOptions();

            // General Capabilities
            PopulateCapabilities(inOptions: result, withWellKnownCapabilities: RecognizedGeneralCapabilities, from: settings?.DeviceSettings?.Options?.GeneralCapabilities);

            // Android Capabilities
            PopulateCapabilities(inOptions: result, withWellKnownCapabilities: RecognizedAndroidCapabilities, from: settings?.DeviceSettings?.Options?.AndroidCapabilities);

            // UIAutomatorCapabilities
            PopulateCapabilities(inOptions: result, withWellKnownCapabilities: RecognizedUIAutomatorCapabilities, from: settings?.DeviceSettings?.Options?.UIAutomatorCapabilities);

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

                var keyIsKnown = withWellKnownCapabilities.ContainsKey(candidateKey);
                if (keyIsKnown)
                {
                    candidateKey = withWellKnownCapabilities.Keys.First(k => string.Compare(k, pair.Key, StringComparison.InvariantCultureIgnoreCase) == 0);
                }

                if (!keyIsKnown)
                {
                    // As the key is not known - do not coerce it. 
                    inOptions.AddAdditionalCapability(candidateKey, pair.Value);
                }
                else
                {
                    inOptions.AddAdditionalCapability(candidateKey, CoerceValueToType(candidateKey, withWellKnownCapabilities[candidateKey], pair.Value));
                }
            });
        }

        private object CoerceValueToType(string candidateKey, string type, object candidateValue)
        {
            var result = candidateValue;

            // Don't be clever if the original value is null - just pass it through
            if (result == null) return result;

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
                case "array":
                    // Leave it as is
                    break;
                case "object":
                    // Leave it as is
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
