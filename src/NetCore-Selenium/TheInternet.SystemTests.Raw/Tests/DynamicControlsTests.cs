using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Linq;

namespace TheInternet.SystemTests.Raw.Tests
{
    [TestClass]
    public class DynamicControlsTests : TheInternetTestBase
    {
        protected override string BaseUrl => base.BaseUrl + "/dynamic_controls";

        public DynamicControlsTests() { }

        [TestMethod]
        public void WhenWeToggleTheCheckbox_ThenTheCheckboxDisappearsAndThenReappears_Raw()
        {
            By checkboxLocator = By.CssSelector("#checkbox");
            Exists(WebDriver, checkboxLocator).Should().BeTrue(because: "the checkbox exists on the page initially");

            var removeButton = WebDriver.FindElements(By.XPath("//button[text()='Remove']")).Single();
            removeButton.Click();

            WebDriver.Assert.Eventually(browser => !Exists(WebDriver, By.CssSelector("#checkbox")));

            var addButton = WebDriver.FindElements(By.XPath("//button[text()='Add']")).Single();
            addButton.Click();
            WebDriver.Assert.Eventually(browser => Exists(browser, By.CssSelector("#checkbox")));
        }

        [TestMethod]
        public void WhenWeToggleTheTextBoxEnabledState_ThenTheTextBoxTogglesBetweenEnabledAndDisabled_Raw()
        {
            WebDriver.FindElements(By.XPath("//input[@type='text']")).Single().Enabled.Should().BeFalse(because: "the text field is initially disabled");

            var enableButton = WebDriver.FindElements(By.XPath("//button[text()='Enable']")).Single();
            enableButton.Click();
            WebDriver.Assert.Eventually(driver => Exists(WebDriver, By.XPath("//input[@type='text' and not(@disabled)]")));

            var enabledTextBox = WebDriver.FindElements(By.XPath("//input[@type='text' and not(@disabled)]")).Single();
            enabledTextBox.SendKeys("Some Text Is Typed");
            enabledTextBox.GetAttribute("value").Should().Be("Some Text Is Typed", because: "the user entered that value");

            var disableButton = WebDriver.FindElements(By.XPath("//button[text()='Disable']")).Single();
            disableButton.Click();
            WebDriver.Assert.Eventually(driver => Exists(WebDriver, By.XPath("//input[@type='text' and @disabled]")));
        }

        private bool Exists(IWebDriver driver, By locator)
        {
            return driver.FindElements(locator).Count() == 1;
        }
    }
}
