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
        protected virtual string BaseUrl => BrowserSession.EnvironmentSettings.BaseUrl;
        protected virtual IWebDriver Browser => BrowserSession.WebDriver;
        protected IBrowserSession BrowserSession { get; private set; }

        [TestInitialize]
        public void SetupSeleniumTest()
        {
            BrowserSession = Resolve<IBrowserSession>();

            BrowserSession.WebDriver.Navigate().GoToUrl(BaseUrl);
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
                BrowserSession?.WebDriver?.Close();
            }
            catch (Exception ex)
            {
                Logger.Error($"{ex}");
            }

            try
            {
                BrowserSession?.WebDriver?.Dispose();
            }
            catch(Exception ex)
            {
                Logger.Error($"{ex}");
            }
        }
    }
}
