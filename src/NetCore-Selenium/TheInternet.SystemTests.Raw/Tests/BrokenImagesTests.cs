using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace TheInternet.SystemTests.Raw.Tests
{
    [TestClass]
    public class BrokenImagesTests : TheInternetTestBase
    {
        public BrokenImagesTests() { }

        protected override string BaseUrl => base.BaseUrl + "/broken_images";

        [TestMethod]
        public void WhenOnAPageWithBrokenImages_ThereAreExactlyTwoBrokenImages()
        {
            FindBrokenImages(WebDriver).Count().Should().Be(2, because: "there are two broken images on the page and at least two good ones");
        }

        protected IEnumerable<IWebElement> FindBrokenImages(IWebDriver browser)
        {
            // Reference: https://stackoverflow.com/questions/43389340/how-to-find-broken-images-for-an-entire-web-site-in-selenium-webdriver-using-jav
            var result = new List<IWebElement>();

            var elements = browser.FindElements(By.CssSelector("img"));
            foreach (var element in elements)
            {
                if (element.GetAttribute("naturalWidth") == "0")
                {
                    result.Add(element);
                }
            }

            return result;
        }

        [TestMethod]
        [Ignore(message: "this will be tackled in Selenium 4: https://github.com/SeleniumHQ/selenium/issues/7342")]
        public void WhenOnAPageWithBrokenImages_TheLogShows404Errors()
        {
            LogsShow404Errors(WebDriver).Should().BeTrue(because: "the favicon and two broken images on the page are missing and the Logs capture those errors");
        }

        protected bool LogsShow404Errors(IWebDriver driver)
        {
            // This is really a sledgehammer approach that just goes through all of the log statements; not recommended because things like favicon.ico
            // will also show up as a 404 and it's not related to the page under test. 
            var errorNumber = "404";
            foreach (var logKind in driver.Manage().Logs.AvailableLogTypes)
            {
                var log = driver.Manage().Logs.GetLog(logKind);
                var hasErrors = log.Where(entry => entry.Level == LogLevel.Severe).Where(entry => entry.Message.Contains(errorNumber)).Count() > 0;
                if (hasErrors)
                {
                    return hasErrors;
                }
            }
            return false;
        }
    }
}
