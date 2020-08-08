using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        }
    }
}
