using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Linq;

namespace TheInternet.SystemTests.Raw.Tests
{
    [TestClass]
    public class DynamicContentTests : TheInternetTestBase
    {
        protected override string BaseUrl => base.BaseUrl + "/dynamic_content";

        public DynamicContentTests() { }

        [TestMethod]
        public void ThenThereShouldBe3Images()
        {
            var images = WebDriver.FindElements(By.CssSelector(".large-2"));
            images.Count().Should().Be(3);
        }

        [TestMethod]
        public void ThenThereShouldBe3TextBlocks()
        {
            var siblingTexts = WebDriver.FindElements(By.XPath("//div[@id='content']/*/div[contains(@class,'large-2')]/following-sibling::div[1]"));
            siblingTexts.Count().Should().Be(3);
        }
    }
}
