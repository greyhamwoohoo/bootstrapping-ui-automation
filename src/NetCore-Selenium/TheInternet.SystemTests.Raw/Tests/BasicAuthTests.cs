using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using static TheInternet.Common.ElementOperations.ElementState;
using static TheInternet.Common.ElementOperations.ElementStateCondition;

namespace TheInternet.SystemTests.Raw.Tests
{
    [TestClass]
    public class BasicAuthTests : TheInternetTestBase
    {
        public BasicAuthTests() { }

        [TestMethod]
        public void WhenWePassCorrectUserNamePasswordInRequest_ThenWeAreAuthenticated_Raw()
        {
            var element = Browser.FindElement(By.XPath("//a[@href='/basic_auth']"));
            var targetUrl = new Uri(element.GetAttribute("href"));

            Browser.Url = GetUriWithBasicAuthentication(targetUrl, "admin", "admin");
            Browser.Navigate();

            Browser.FindElement(By.XPath("//h3[text()='Basic Auth']"));
        }

        [TestMethod]
        public void WhenWePassCorrectUserNamePasswordInRequest_ThenWeAreAuthenticated_Waiter()
        {
            // Assumption
            var basicAuthLinkLocator = By.XPath("//a[@href='/basic_auth']");
            var basicAuthPageContentLocator = By.XPath("//h3[text()='Basic Auth']");

            BrowserSession.Waiter.AssertThatEventually(basicAuthLinkLocator, IsEnabled);

            // Arrange
            var element = Browser.FindElement(basicAuthLinkLocator);
            var targetUrl = new Uri(element.GetAttribute("href"));

            // Act
            Browser.Url = GetUriWithBasicAuthentication(targetUrl, "admin", "admin");
            Browser.Navigate();

            // Assert
            BrowserSession.Waiter.AssertThatEventually(basicAuthPageContentLocator, IsDisplayed);
        }

        protected string GetUriWithBasicAuthentication(Uri uri, string username, string password)
        {
            return $"{uri.Scheme}://{username}:{password}@{uri.AbsoluteUri.Substring(uri.Scheme.Length + 3)}";
        }

    }
}
