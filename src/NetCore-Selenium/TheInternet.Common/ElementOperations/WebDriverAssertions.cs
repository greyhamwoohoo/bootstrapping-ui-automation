using OpenQA.Selenium;
using System;
using System.Linq;
using TheInternet.Common.ElementOperations.Contracts;
using TheInternet.Common.ExecutionContext.Runtime.ControlSettings;
using TheInternet.Common.Reporting.Contracts;

namespace TheInternet.Common.ElementOperations
{
    public class WebDriverAssertions : IWebDriverAssertions
    {
        private readonly IDecoratedWebDriver _webDriver;
        private readonly IControlSettings _controlSettings;
        private readonly ITestCaseReporter _testCaseReporter;

        public WebDriverAssertions(IDecoratedWebDriver webDriver, IControlSettings controlSettings, ITestCaseReporter testCaseReporter)
        {
            _testCaseReporter = testCaseReporter ?? throw new System.ArgumentNullException(nameof(testCaseReporter));
            _webDriver = webDriver ?? throw new System.ArgumentNullException(nameof(webDriver)); ; 
            _controlSettings = controlSettings ?? throw new System.ArgumentNullException(nameof(controlSettings)); ;
        }

        public IWebElement Click(By locator)
        {
            return Click(locator, because: $"{typeof(IWebDriverAssertions).Name}.Click({locator})");
        }

        public IWebElement Click(By locator, string because)
        {
            IWebElement element = default;

            AssertThatEventually(driver =>
            {
                element = AssertExactlyOneElementExists(_webDriver, locator);

                element.Click();
            }, because);

            return element;
        }

        public IWebElement Type(By locator, string keys, bool andPressEnter)
        {
            return Type(locator, keys, andPressEnter, because: $"{typeof(IWebDriverAssertions).Name}.Type({locator}, keys: '{keys}', andPressEnter: {andPressEnter})");
        }

        public IWebElement Type(By locator, string keys, bool andPressEnter, string because)
        {
            IWebElement element = default;

            AssertThatEventually(driver =>
            {
                element = AssertExactlyOneElementExists(_webDriver, locator);

                element.SendKeys(keys);
                if(andPressEnter)
                {
                    element.SendKeys(Keys.Enter);
                }
            }, because);

            return element;
        }

        private IWebElement AssertExactlyOneElementExists(IDecoratedWebDriver webDriver, By locator)
        {
            var elements = webDriver.FindElements(locator);
            if (elements.Count() == 0) throw new NotFoundException($"The element {locator} could not be found. ");
            if (elements.Count() > 1) throw new NotFoundException($"The element {locator} was found {elements.Count()} times instead of exactly once. ");

            return elements.Single();
        }

        /// <summary>
        /// Wait until the expected callback DOES NOT throw an exception. 
        /// The callback usually contains one or more assertions. 
        /// </summary>
        /// <param name="callback"></param>
        private void AssertThatEventually(Action<IDecoratedWebDriver> callback, string because)
        {
            if (null == callback) throw new System.ArgumentNullException(nameof(callback));

            DateTime endMatch = DateTime.Now.AddSeconds(_controlSettings.WaitUntilTimeoutInSeconds);
            Exception lastException = default(Exception);

            do
            {
                try
                {
                    callback(_webDriver);
                    _testCaseReporter.Pass(because);
                    return;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                }

                System.Threading.Thread.Sleep(_controlSettings.PollingTimeInMilliseconds);

            } while (DateTime.Now < endMatch);

            _testCaseReporter.Fail(because, lastException);

            throw lastException;
        }
    }
}
