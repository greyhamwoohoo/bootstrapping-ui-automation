using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace TheInternet.SystemTests.Raw
{
    /// <summary>
    /// Use this single test as a scratch pad for reusing a browser instance. 
    /// </summary>
    /// <remarks>
    /// If using Visual Studio, be sure to select Test / Configure Run Settings / Configure Solution Wide runsettings file and select 'TestExecutionContexts\Attachable-Chrome-Localhost.runsettings'
    /// Run the test once; it will open the browser
    /// Add a few lines of Selenium at a time; re-run the test and it will reuse the same browser instance
    /// ---
    /// To automatically run the test every time this file is saved, do this from a command line from this folder:
    /// 
    /// SET THEINTERNET_TEST_EXECUTION_CONTEXT=attachable-chrome-localhost
    /// dotnet watch test --filter "Name="HotReloadWorkflow"
    /// </remarks>
    [TestClass]
    public class HotReloadScratchpad : SeleniumTestBase
    {
        protected override void NavigateToBaseUrl()
        {
            // We do nothing here - we want to control everything 
        }

        [TestMethod]
        [TestCategory("HotReloadWorkflow")]
        public void HotReloadWorkflow()
        {
            // Browser.Navigate().GoToUrl("http://www.google.com");
            // Browser.FindElement(By.Name("q")).SendKeys("hello!" + Keys.Enter);
        }
    }
}
