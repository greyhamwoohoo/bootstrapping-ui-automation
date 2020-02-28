using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TheInternet.Common.SessionManagement.Contracts;

namespace TheInternet.SystemTests.Raw
{
    [TestClass]
    public abstract class SeleniumTestBase : TestBase
    {
        protected virtual string BaseUrl => BrowserSession.EnvironmentSettings.BaseUrl;
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
            try
            {
                BrowserSession?.WebDriver?.Close();
            }
            catch (Exception ex)
            {
                // TODO: Logger
            }

            try
            {
                BrowserSession?.WebDriver?.Dispose();
            }
            catch(Exception ex)
            {
                // TODO: Logger
            }
        }
    }
}
