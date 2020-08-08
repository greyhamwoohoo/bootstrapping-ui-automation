using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace TheInternet.Common.SystemTests
{
    [TestClass]
    public class SmokeTests : SeleniumTestBase
    {
        protected override string BaseUrl => "https://www.google.com";

        [TestMethod]
        public void Google_Can_Be_Reached()
        {
            WebDriver.Assert.Click(By.Name("q"));
        }
    }
}
