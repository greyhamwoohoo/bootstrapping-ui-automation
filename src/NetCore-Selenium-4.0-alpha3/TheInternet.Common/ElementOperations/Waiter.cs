using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheInternet.Common.ExecutionContext.Runtime.ControlSettings;

namespace TheInternet.Common.ElementOperations
{
    /// <summary>
    /// Eventual Consistency Helpers
    /// </summary>
    /// <remarks>Wait for multiple controls and their state to converge. 
    /// </remarks>
    public class Waiter : IWaiter
    {
        public int WaitUntilTimeoutInSeconds => _controlSettings.WaitUntilTimeoutInSeconds;
        public int PollingTimeInMilliseconds => _controlSettings.PollingTimeInMilliseconds;

        private IWebDriver _driver;
        private ILogger _logger;
        private IControlSettings _controlSettings;

        public Waiter(IWebDriver driver, ILogger logger, IControlSettings controlSettings)
        {
            if (null == driver) throw new System.ArgumentNullException(nameof(driver));
            if (null == logger) throw new System.ArgumentNullException(nameof(logger));
            if (null == controlSettings) throw new System.ArgumentNullException(nameof(controlSettings));

            _driver = driver;
            _logger = logger;
            _controlSettings = controlSettings;
        }

        /// <summary>
        /// Wait until the expected condition is true. 
        /// </summary>
        /// <param name="callback">Callback. </param>
        public void AssertThatEventually(Func<IWebDriver, bool> callback)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(WaitUntilTimeoutInSeconds));
            wait.Until(browser => callback(browser));
        }

        /// <summary>
        /// Wait until the expected callback DOES NOT throw an exception. 
        /// The callback usually contains one or more assertions. 
        /// </summary>
        /// <param name="callback"></param>
        public void AssertThatEventually(Action<IWebDriver> callback)
        {
            DateTime endMatch = DateTime.Now.AddSeconds(WaitUntilTimeoutInSeconds);
            Exception lastException = default(Exception);

            do
            {
                try
                {
                    callback(_driver);
                    return;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                }

                // NOTE: We have AT LEAST one exception. 
                System.Threading.Thread.Sleep(PollingTimeInMilliseconds);

            } while (DateTime.Now < endMatch);

            throw lastException;
        }

        /// <summary>
        /// Wait until the element matches the given state. 
        /// </summary>
        /// <param name="element">Element to match</param>
        /// <param name="state">The state to match</param>
        /// <returns></returns>
        public IWebElement AssertThatEventually(By element, ElementState state)
        {
            return AssertThatEventually(new ElementStateCondition() { Locator = element, State = state });
        }

        /// <summary>
        /// Wait until all of the conditions match; or throw an exception if one or more of the conditions cannot be matched. 
        /// </summary>
        /// <param name="element">Search criteria</param>
        /// <param name="state">The state we are searching for</param>
        /// <param name="conditions">Conditions to match</param>
        public void AssertThatEventually(By element, ElementState state, params ElementStateCondition[] conditions)
        {
            var temp = new List<ElementStateCondition>();
            temp.Add(new ElementStateCondition() { Locator = element, State = state });
            temp.AddRange(conditions);

            AssertThatEventually(temp);
        }

        /// <summary>
        /// Handler for the specific and most common use case - one match. 
        /// </summary>
        /// <param name="condition">The condition to match</param>
        /// <returns>The WebElement matching the condition; or any number of Selenium and generic exceptions. </returns>
        public IWebElement AssertThatEventually(ElementStateCondition condition)
        {
            AssertThatEventually(new ElementStateCondition[] { condition });
            return Match(condition);
        }

        /// <summary>
        /// Wait until all of the elements match or the timeout is exceeded. 
        /// </summary>
        /// <param name="conditions">The conditions to match. </param>
        public void AssertThatEventually(IEnumerable<ElementStateCondition> conditions)
        {
            var foundElements = new List<ElementStateCondition>();
            var exceptions = new List<Exception>();

            DateTime endMatch = DateTime.Now.AddSeconds(WaitUntilTimeoutInSeconds);

            _logger.Information($"WaitUntil:Conditions:Dump");
            conditions.ToList().ForEach(condition => _logger.Information($"{condition}"));

            do
            {
                foundElements.Clear();
                exceptions.Clear();
                foreach (var condition in conditions)
                {
                    try
                    {
                        _logger.Information($"Match:Condition:Begin:{condition}");
                        var match = Match(condition);
                        _logger.Information($"Match:Condition:Success:{condition}");
                        foundElements.Add(condition);
                    }
                    catch (Exception ex)
                    {
                        _logger.Information($"Match:Condition:Fail:{condition}:{ex.GetType().FullName}");
                        exceptions.Add(ex);
                    }
                }

                if (foundElements.Count == conditions.Count()) return;

                // NOTE: We have AT LEAST one exception. 
                System.Threading.Thread.Sleep(PollingTimeInMilliseconds);

            } while (DateTime.Now < endMatch);

            // ASSERTION: At least one of the elements did not match. Dump out all the information we have about the matching. 
            var builder = new StringBuilder();

            var notFoundElements = conditions.Except(foundElements);
            if (notFoundElements.Count() > 0)
            {
                builder.AppendFormat("The following elements were not in the expected state: \r\n");
                notFoundElements.ToList().ForEach(notFoundElement =>
                {
                    builder.AppendFormat($"{notFoundElement.Locator.ToString()}\r\n");
                });
                builder.AppendFormat("\r\n");
            }

            if (foundElements.Count > 0)
            {
                builder.AppendFormat("The following elements were in the expected state: \r\n");
                foundElements.ForEach(foundElement =>
                {
                    builder.AppendFormat($"{foundElement.Locator.ToString()}\r\n");
                });
                builder.AppendFormat("\r\n");
            }

            if (exceptions.Count > 0)
            {
                builder.AppendFormat("Exception Information for elements not in the expected state: \r\n");
                exceptions.ForEach(exception =>
                {
                    builder.AppendFormat($"{exception.ToString()}\r\n");
                });
            }

            _logger.Error(builder.ToString());
            throw new NoSuchElementException(builder.ToString());
        }

        /// <summary>
        /// Immediately matches the current condition by trying to locate the element. 
        /// </summary>
        /// <param name="condition">Condition to match</param>
        /// <returns>The element if it exists; will throw an exception if the element cannot be matched. </returns>
        protected IWebElement Match(ElementStateCondition condition)
        {
            // TODO: Use custom exceptions here
            var webElement = default(IWebElement);

            if (condition.State.HasFlag(ElementState.Exists))
            {
                var webElements = _driver.FindElements(condition.Locator);
                if (webElements.Count() == 0) throw new NoSuchElementException($"{condition.Locator}");
                if (webElements.Count() != 1) throw new System.InvalidOperationException($"ERROR: There is more than one instance of {condition.State.ToString()}. There must be exactly one instance on the page. ");

                webElement = webElements[0];
            }

            if (condition.State.HasFlag(ElementState.IsDisplayed))
            {
                if (!webElement.Displayed) throw new NoSuchElementException($"The Element is not visible", new OpenQA.Selenium.ElementNotVisibleException($"The Element is not visible: {webElement}"));
            }

            if (condition.State.HasFlag(ElementState.IsEnabled))
            {
                if (!webElement.Enabled) throw new NoSuchElementException($"The Element is not enabled", new OpenQA.Selenium.ElementNotInteractableException($"The Element is not enabled: {webElement}"));
            }

            return webElement;
        }
    }
}
