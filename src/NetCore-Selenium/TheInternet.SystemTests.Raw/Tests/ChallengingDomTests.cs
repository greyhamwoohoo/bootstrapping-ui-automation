using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Linq;
using static Yasf.Common.ElementOperations.ElementState;
using static Yasf.Common.ElementOperations.ElementStateCondition;

namespace TheInternet.SystemTests.Raw.Tests
{
    [TestClass]
    public class ChallengingDomTests : TheInternetTestBase
    {
        protected override string BaseUrl => base.BaseUrl + "/challenging_dom";

        public ChallengingDomTests() { }

        [TestMethod]
        public void WhenThePageIsLoaded_ThenTheButtonsCanAlwaysBeIdentifiedByStyle_Raw()
        {
            // Implicitly: Single will throw if there is not exactly one element
            var notAlertAndNotSuccessButton = WebDriver.FindElements(By.CssSelector(".button:not(.alert):not(.success)")).Single();
            var alertButton = WebDriver.FindElements(By.CssSelector(".button.alert")).Single();
            var successButton = WebDriver.FindElements(By.CssSelector(".button.success")).Single();
        }

        [TestMethod]
        public void WhenThePageIsLoaded_ThenTheButtonsCanAlwaysBeIdentifiedByStyle_Waiter()
        {
            var notAlertAndNotSuccessButtonLocator = By.CssSelector(".button:not(.alert):not(.success)");
            var alertButtonLocator = By.CssSelector(".button.alert");
            var successButtonLocator = By.CssSelector(".button.success");

            // Assume
            WebDriver.Assert.State(notAlertAndNotSuccessButtonLocator, IsDisplayed,
                And(alertButtonLocator, IsDisplayed),
                And(successButtonLocator, IsDisplayed));
        }

        [TestMethod]
        public void WeWantToSelectByWhenWeWantToEditARow_ThenWeCanEditADiceret_Raw()
        {
            WebDriver.Url.Should().NotEndWith("#edit", because: "we will navigate to that location during the test");

            var phaeDrum4Cell = WebDriver.FindElements(By.XPath("//td[text()='Phaedrum4']")).Single();
            var phaeDrum4Row = WebDriver.FindElements(By.XPath("//td[text()='Phaedrum4']/ancestor::tr[1]")).Single();

            var editRowElement = phaeDrum4Row.FindElements(By.XPath(".//*/a[text()='edit']")).Single();
            editRowElement.Click();
            WebDriver.Url.Should().EndWith("#edit", because: "the 'edit' cell was clicked");
        }

        [TestMethod]
        public void WeWantToSelectByWhenWeWantToEditARow_ThenWeCanEditADiceret_Waiter()
        {
            var phaeDrum4CellLocator = By.XPath("//td[text()='Phaedrum4']");
            var phaeDrum4RowLocator = By.XPath("//td[text()='Phaedrum4']/ancestor::tr[1]");

            // Assume
            WebDriver.Url.Should().NotEndWith("#edit", because: "we will navigate to that location during the test");
            WebDriver.Assert.State(phaeDrum4CellLocator, IsDisplayed,
                And(phaeDrum4RowLocator, IsDisplayed));

            // Arrange
            By editRowElementLocator = By.XPath(".//*/a[text()='edit']");

            // Act
            var phaeDrum4Row = WebDriver.Assert.State(phaeDrum4RowLocator, IsDisplayed);
            var editRowElement = phaeDrum4Row.FindElements(editRowElementLocator).Single();
            editRowElement.Click();

            // Assert
            WebDriver.Url.Should().EndWith("#edit", because: "the 'edit' cell was clicked");
        }

        [TestMethod]
        public void WeWantToSelectByWhenWeWantToEditARow_ThenWeCanDeleteADiceret_Raw()
        {
            WebDriver.Url.Should().NotEndWith("#delete", because: "we will navigate to that location during the test");

            var phaeDrum4Cell = WebDriver.FindElements(By.XPath("//td[text()='Phaedrum4']")).Single();
            var phaeDrum4Row = WebDriver.FindElements(By.XPath("//td[text()='Phaedrum4']/ancestor::tr[1]")).Single();

            var deleteRowElement = phaeDrum4Row.FindElements(By.XPath(".//*/a[text()='delete']")).Single();
            deleteRowElement.Click();
            WebDriver.Url.Should().EndWith("#delete", because: "the 'delete' cell was clicked");
        }

        [TestMethod]
        public void WeWantToSelectByWhenWeWantToEditARow_ThenWeCanDeleteADiceret_Waiter()
        {
            var phaeDrum4CellLocator = By.XPath("//td[text()='Phaedrum4']");
            var phaeDrum4RowLocator = By.XPath("//td[text()='Phaedrum4']/ancestor::tr[1]");

            // Assume
            WebDriver.Url.Should().NotEndWith("#delete", because: "we will navigate to that location during the test");
            WebDriver.Assert.State(phaeDrum4CellLocator, IsDisplayed,
                And(phaeDrum4RowLocator, IsDisplayed));

            // Arrange
            var phaeDrum4Row = WebDriver.Assert.State(phaeDrum4RowLocator, IsDisplayed);

            // Act
            By deleteRowElementLocator = By.XPath(".//*/a[text()='delete']");
            var deleteRowElement = phaeDrum4Row.FindElements(deleteRowElementLocator).Single();
            deleteRowElement.Click();

            // Assert
            WebDriver.Url.Should().EndWith("#delete", because: "the 'delete' cell was clicked");
        }
        // TODO: Test Canvas; we could pull it from the page source with a RegEx (or use Tesseract?)
    }
}
