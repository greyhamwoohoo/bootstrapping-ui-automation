using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Linq;
using static TheInternet.Common.ElementOperations.ElementState;
using static TheInternet.Common.ElementOperations.ElementStateCondition;

namespace TheInternet.SystemTests.Raw.Tests
{
    [TestClass]
    public class DynamicLoadedPageElementsTests : TheInternetTestBase
    {
        protected override string BaseUrl => base.BaseUrl + "/dynamic_loading";
        
        public DynamicLoadedPageElementsTests() { }

        [TestMethod]
        public void GivenAHiddenElement_WhenAnElementIsShown_ItWillBecomeVisible()
        {
            var example1 = WebDriver.FindElements(By.XPath("//a[contains(text(), 'Example 1')]")).Single();
            WebDriver.Navigate().GoToUrl(example1.GetAttribute("href"));

            var startButton = WebDriver.FindElements(By.XPath("//button[text()='Start']")).Single();
            startButton.Click();

            var finishLocator = By.CssSelector("#finish");
            WebDriver.Assert.State(finishLocator, IsDisplayed);
        }

        [TestMethod]
        public void GivenAHiddenElement_WhenAnElementIsShown_ItWillBeRenderedBecomeVisible()
        {
            var example2 = WebDriver.FindElements(By.XPath("//a[contains(text(), 'Example 2')]")).Single();
            WebDriver.Navigate().GoToUrl(example2.GetAttribute("href"));

            var startButton = WebDriver.FindElements(By.XPath("//button[text()='Start']")).Single();
            startButton.Click();

            var finishLocator = By.CssSelector("#finish");
            WebDriver.Assert.State(finishLocator, IsDisplayed);
        }
    }
}
