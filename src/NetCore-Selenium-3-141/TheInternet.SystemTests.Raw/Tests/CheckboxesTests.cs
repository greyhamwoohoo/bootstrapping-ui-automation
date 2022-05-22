using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Linq;

namespace TheInternet.SystemTests.Raw.Tests
{
    [TestClass]
    public class CheckboxesTests : TheInternetTestBase
    {
        protected override string BaseUrl => base.BaseUrl + "/checkboxes";
        
        public CheckboxesTests()
        {
        }

        [TestMethod]
        public void WillFindCheckboxesByXPath_Raw()
        {
            var checkboxes = WebDriver.FindElements(By.XPath("//input[@type='checkbox']"));
            checkboxes.Count().Should().Be(2, because: "there are two checkboxes on the screen");
        }

        [TestMethod]
        public void WillFindCheckboxesByCss_Raw()
        {
            var checkboxes = WebDriver.FindElements(By.CssSelector("input[type='checkbox']"));
            checkboxes.Count().Should().Be(2, because: "there are two checkboxes on the screen");
        }

        [TestMethod]
        public void WillFindSelectedCheckboxesByCss_Raw()
        {
            var checkboxes = WebDriver.FindElements(By.CssSelector("input[type='checkbox']:checked"));
            checkboxes.Count().Should().Be(1, because: "only a single checkbox is selected");
        }

        [TestMethod]
        public void WillFindCheckboxesByTextAndThenToggle_Raw()
        {
            // The structure of the Checkboxes is:
            // text
            // input checkbox
            // text (checkbox 1)
            // input checkbox
            // text (checkbox 2)
            //
            // So - crudely - find the checkbox text and then pick the first input before it. 
            var checkbox1Text = "checkbox 1";
            var checkbox1 = WebDriver.FindElements(By.XPath($"//form[@id='checkboxes']/node()[contains(. ,'{checkbox1Text}')]/preceding-sibling::input[1]")).Single();
            checkbox1.Selected.Should().BeFalse(because: "the first checkbox is not selected by default");

            var checkbox2Text = "checkbox 2";
            var checkbox2 = WebDriver.FindElements(By.XPath($"//form[@id='checkboxes']/node()[contains(. ,'{checkbox2Text}')]/preceding-sibling::input[1]")).Single();
            checkbox2.Selected.Should().BeTrue(because: "second checkbox is selected by default");

            checkbox1.Click();
            checkbox1.Selected.Should().BeTrue(because: "the first checkbox should now be toggled on");

            checkbox2.Click();
            checkbox2.Selected.Should().BeFalse(because: "second checkbox is not selected anymore");
        }

        [TestMethod]
        public void WillFindCheckboxesByTextAndThenToggle_Raw_Eventually()
        {
            // The structure of the Checkboxes is:
            // text
            // input checkbox
            // text (checkbox 1)
            // input checkbox
            // text (checkbox 2)
            //
            // So - crudely - find the checkbox text and then pick the first input before it. 
            var checkbox1Text = "checkbox 1";
            var checkbox1Locator = By.XPath($"//form[@id='checkboxes']/node()[contains(. ,'{checkbox1Text}')]/preceding-sibling::input[1]");
            var checkbox1 = WebDriver.FindElements(checkbox1Locator).Single();
            checkbox1.Selected.Should().BeFalse(because: "the first checkbox is not selected by default");

            var checkbox2Text = "checkbox 2";
            var checkbox2Locator = By.XPath($"//form[@id='checkboxes']/node()[contains(. ,'{checkbox2Text}')]/preceding-sibling::input[1]");
            var checkbox2 = base.WebDriver.FindElements(checkbox2Locator).Single();
            checkbox2.Selected.Should().BeTrue(because: "second checkbox is selected by default");

            WebDriver.Assert.Click(checkbox1Locator);
            checkbox1.Selected.Should().BeTrue(because: "the first checkbox should now be toggled on");

            WebDriver.Assert.Click(checkbox2Locator);
            checkbox2.Selected.Should().BeFalse(because: "second checkbox is not selected anymore");
        }
    }
}
