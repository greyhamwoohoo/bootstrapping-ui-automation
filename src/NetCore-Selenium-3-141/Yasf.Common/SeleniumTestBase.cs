using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Yasf.Common.ElementOperations.Contracts;
using Yasf.Common.ExecutionContext.Runtime.ControlSettings;
using Yasf.Common.Reporting.Contracts;
using Yasf.Common.SessionManagement.Contracts;

namespace Yasf.Common
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
        protected ITestCaseReporter Reporter => TestCaseReporter;
        private ITestCaseReporter TestCaseReporter => DriverSession.TestCaseReporter;


        [TestInitialize]
        public void SetupSeleniumTest()
        {
            DriverSession = Resolve<IDriverSession>();

            TestCaseReporter.Initialize(DriverSession, TestContext.TestName);

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
                CloseDriverSessionWebDriver();
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
                var logPath = TestCaseReporter.LogFilePath;
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

        protected virtual void CloseDriverSessionWebDriver()
        {
            DriverSession?.WebDriver?.Close();
        }
    }
}
