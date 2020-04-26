using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using TheInternet.Common.ExecutionContext.Runtime.ControlSettings;
using TheInternet.Common.SessionManagement.Contracts;

namespace TheInternet.SystemTests.Raw
{
    [TestClass]
    public abstract class SeleniumTestBase : TestBase
    {
        protected virtual string BaseUrl => DriverSession.EnvironmentSettings.BaseUrl;
        protected virtual IWebDriver Browser => DriverSession.WebDriver;
        protected IDriverSession DriverSession { get; private set; }

        [TestInitialize]
        public void SetupSeleniumTest()
        {
            DriverSession = Resolve<IDriverSession>();

            NavigateToBaseUrl();
        }

        [TestCleanup]
        public void TeardownSeleniumTest()
        {
            if(Resolve<IControlSettings>().AttachToExistingSessionIfItExists)
            {
                Logger.Information($"The Control Settings want to attach to an existing session if it exists. Therefore, we will not close the browser at this time. ");
                return;
            }

            try
            {
                DriverSession?.WebDriver?.Close();
            }
            catch (Exception ex)
            {
                Logger.Error($"{ex}");
            }

            try
            {
                DriverSession?.WebDriver?.Dispose();
            }
            catch(Exception ex)
            {
                Logger.Error($"{ex}");
            }
        }

        protected virtual void NavigateToBaseUrl()
        {
            DriverSession.WebDriver.Navigate().GoToUrl(BaseUrl);
        }
    }
}
