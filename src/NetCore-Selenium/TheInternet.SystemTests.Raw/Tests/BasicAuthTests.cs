using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using static TheInternet.Common.ElementOperations.ElementState;

namespace TheInternet.SystemTests.Raw.Tests
{
    [TestClass]
    public class BasicAuthTests : TheInternetTestBase
    {
        public BasicAuthTests() { }

        [TestMethod]
        public void WhenWePassCorrectUserNamePasswordInRequest_ThenWeAreAuthenticated_Raw()
        {
            var element = WebDriver.FindElement(By.XPath("//a[@href='/basic_auth']"));
            var targetUrl = new Uri(element.GetAttribute("href"));

            WebDriver.Url = GetUriWithBasicAuthentication(targetUrl, "admin", "admin");
            WebDriver.Navigate();

            WebDriver.FindElement(By.XPath("//h3[text()='Basic Auth']"));
        }

        [TestMethod]
        public void WhenWePassCorrectUserNamePasswordInRequest_ThenWeAreAuthenticated_Waiter()
        {
            // Assumption
            var basicAuthLinkLocator = By.XPath("//a[@href='/basic_auth']");
            var basicAuthPageContentLocator = By.XPath("//h3[text()='Basic Auth']");

            WebDriver.Assert.State(basicAuthLinkLocator, IsEnabled);

            // Arrange
            var element = WebDriver.FindElement(basicAuthLinkLocator);
            var targetUrl = new Uri(element.GetAttribute("href"));

            // Act
            WebDriver.Url = GetUriWithBasicAuthentication(targetUrl, "admin", "admin");
            WebDriver.Navigate();

            // Assert
            WebDriver.Assert.State(basicAuthPageContentLocator, IsDisplayed);
        }

        protected string GetUriWithBasicAuthentication(Uri uri, string username, string password)
        {
            return $"{uri.Scheme}://{username}:{password}@{uri.AbsoluteUri.Substring(uri.Scheme.Length + 3)}";
        }
    }
}
