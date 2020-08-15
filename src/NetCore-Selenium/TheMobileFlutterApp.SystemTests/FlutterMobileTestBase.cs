using GreyhamWooHoo.Flutter;
using GreyhamWooHoo.Flutter.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Remote;
using TheInternet.Common;
using TheInternet.Common.ExecutionContext.Runtime.ControlSettings;

namespace TheMobileFlutterApp.SystemTests
{
    [TestClass]
    public abstract class FlutterMobileTestBase : AppiumTestBase
    {
        protected IFlutterDriver FlutterDriver { get; private set; }

        [TestInitialize]
        public void SetupFlutterMobileTest()
        {
            IWrapsDriver wrapsDriver = WebDriver as IWrapsDriver;

            var remoteWebDriver = wrapsDriver?.WrappedDriver as RemoteWebDriver;
            if (null == remoteWebDriver) throw new System.InvalidOperationException($"FlutterDriver is intended to wrap AppiumDrivers");

            FlutterDriver = new FlutterDriver(remoteWebDriver, Resolve<ICommandExecutor>(), Resolve<IControlSettings>().WaitUntilTimeoutInSeconds);
        }

        [TestCleanup]
        public void CleanupFlutterMobileTest()
        {

        }

        protected override void CloseDriverSessionWebDriver()
        {
            Logger.Information($"WebDriver.Close() is not implemented on 'appium-flutter-driver' so will be skipped. ");
        }
    }
}
