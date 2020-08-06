using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace TheInternet.Common.ElementOperations.Contracts
{
    public interface IWaiter
    {
        void AssertThatEventually(Action<IWebDriver> callback);
        IWebElement AssertThatEventually(By element, ElementState state);
        void AssertThatEventually(By element, ElementState state, params ElementStateCondition[] conditions);
        IWebElement AssertThatEventually(ElementStateCondition condition);
        void AssertThatEventually(Func<IWebDriver, bool> callback);
        void AssertThatEventually(IEnumerable<ElementStateCondition> conditions);
    }
}