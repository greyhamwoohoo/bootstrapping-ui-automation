using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using static TheInternet.Common.ElementOperations.ElementState;
using static TheInternet.Common.ElementOperations.ElementStateCondition;

namespace TheInternet.SystemTests.Raw.Tests
{
    [TestClass]
    public class DropdownListTests : TheInternetTestBase
    {
        private IWebElement _dropdown;
        private SelectElement _selectElement;

        protected override string BaseUrl => base.BaseUrl + "/dropdown";
        
        public DropdownListTests() { }

        [TestInitialize]
        public void SetupDropdownList()
        {
            _dropdown = Browser.FindElements(By.CssSelector("#dropdown")).Single();
            _selectElement = new SelectElement(_dropdown);
        }

        [TestMethod]
        public void SetupDropdownList_Waiter()
        {
            By dropdownLocator = By.CssSelector("#dropdown");

            var dropdown = BrowserSession.Waiter.AssertThatEventually(dropdownLocator, IsDisplayed);
            _selectElement = new SelectElement(dropdown);
        }

        [TestMethod]
        public void TheOptionsAreInitiallyDisabled()
        {
            _selectElement.SelectedOption.Text.Should().Be("Please select an option", because: "nothing has been selected yet");
            _selectElement.SelectedOption.Enabled.Should().BeFalse(because: "the first item is not enabled");
            _selectElement.AllSelectedOptions.Count().Should().Be(1, because: "although disabled, it is the first selected entry");
        }

        [TestMethod]
        public void CanSelectByIndex()
        {
            _selectElement.SelectByIndex(1);
            _selectElement.SelectedOption.Enabled.Should().BeTrue(because: "the second entry is enabled");
            _selectElement.SelectedOption.Text.Should().Be("Option 1", because: "the first entry is selected");
        }

        [TestMethod]
        public void CanSelectByText()
        {
            _selectElement.SelectByText("Option 2");
            _selectElement.SelectedOption.Enabled.Should().BeTrue(because: "the third entry is enabled");
            _selectElement.SelectedOption.Text.Should().Be("Option 2", because: "the second entry is selected");
        }
    }
}
