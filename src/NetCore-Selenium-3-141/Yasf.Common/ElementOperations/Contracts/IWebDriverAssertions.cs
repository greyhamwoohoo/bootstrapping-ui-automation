using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Yasf.Common.ElementOperations.Contracts
{
    /// <summary>
    /// All Assertions use an implementation similar to "ImplicitWait" - in other words: the operation 
    /// </summary>
    /// <remarks>
    /// </remarks>
    public interface IWebDriverAssertions
    {
        IWebElement Click(By locator);
        IWebElement Click(By locator, string because);
        IWebElement Type(By locator, string keys, bool andPressEnter);
        IWebElement Type(By locator, string keys, bool andPressEnter, string because);
        IWebElement State(By element, ElementState state);
        void State(By element, ElementState state, params ElementStateCondition[] conditions);
        IWebElement State(ElementStateCondition condition);
        void State(IEnumerable<ElementStateCondition> conditions);
        void Eventually(Action<IWebDriver> callback);
        void Eventually(Action<IWebDriver> callback, string because);
        public void Eventually(Func<IWebDriver, bool> callback);
    }
}
