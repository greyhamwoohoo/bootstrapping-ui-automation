using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheInternet.Common;
using TheInternet.Common.ExecutionContext.Runtime.BrowserSettings.Contracts;

namespace TheInternet.SystemTests.Raw
{
    [TestClass]
    public class SmokeTest : SeleniumTestBase
    {
        protected override string BaseUrl => "https://www.google.com";

        [TestMethod]
        public void Will_Log_One_Of_Each_Level()
        {
            Reporter.Debug("Debug");
            Reporter.Information("Information");
            Reporter.Warning("Warning");
            Reporter.Error("Error");
            Reporter.Error("MyException", new System.IO.FileNotFoundException($"The file was not found"));
        }
    }
}
