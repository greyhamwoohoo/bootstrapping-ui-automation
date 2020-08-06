using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TheInternet.Common.ElementOperations.Contracts;
using TheInternet.Common.ExecutionContext.Runtime.ControlSettings;
using TheInternet.Common.Reporting.Contracts;
using TheInternet.Common.SessionManagement.Contracts;

namespace TheInternet.Common
{
    /// <summary>
    /// Base class for all Web based tests: use for Selenium and Device Web Browsing
    /// </summary>
    [TestClass]
    public abstract class SeleniumTestBase : TestBase
    {
        protected virtual string BaseUrl => DriverSession.EnvironmentSettings.BaseUrl;
        protected virtual IDecoratedWebDriver WebDriver => DriverSession.WebDriver;
        protected IDriverSession DriverSession { get; private set; }
        protected ITestRunReporter TestRunReporter { get; private set; }
        protected ITestCaseReporter TestCaseReporter { get; private set; }

        [TestInitialize]
        public void SetupSeleniumTest()
        {
            DriverSession = Resolve<IDriverSession>();

            TestCaseReporter = Resolve<ITestCaseReporter>();
            TestCaseReporter.Initialize(TestContext.TestName);

            NavigateToBaseUrl();
        }

        [TestCleanup]
        public void TeardownSeleniumTest()
        {
            if (Resolve<IControlSettings>().AttachToExistingSessionIfItExists)
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
            catch (Exception ex)
            {
                Logger.Error($"{ex}");
            }

            try
            {
                var logPath = TestCaseReporter.LogPath;
                if (System.IO.File.Exists(logPath))
                {
                    TestContext.AddResultFile(logPath);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"{ex}");
            }

            try
            {
                TestCaseReporter.Uninitialize();
            }
            catch (Exception ex)
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
