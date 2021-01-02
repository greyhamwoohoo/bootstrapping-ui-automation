using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using TheInternet.Common;

namespace TheInternet.SystemTests.Raw
{
    [TestClass]
    public class SmokeTests : SeleniumTestBase
    {
        protected override string BaseUrl => "https://www.google.com";

        [TestMethod]
        public void Can_Report_Each_Level()
        {
            Reporter.Debug("Debug");
            Reporter.Information("Information");
            Reporter.Warning("Warning");
            Reporter.Error("Error");
            Reporter.Error("MyException", new System.IO.FileNotFoundException($"The file was not found"));
            Reporter.Pass("This passed");
            Reporter.Fail("This failed");
            Reporter.Fail("This failed", new System.IO.FileNotFoundException($"With an exception"));
        }
    }
}
