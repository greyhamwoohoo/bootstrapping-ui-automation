using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Linq;

namespace TheInternet.SystemTests.Raw.Tests
{
    [TestClass]
    public class DisappearingElementsTests : TheInternetTestBase
    {
        protected override string BaseUrl => base.BaseUrl + "/disappearing_elements";
        
        public DisappearingElementsTests() { }

        [TestMethod]
        public void WillDetermineIfAllThingsExist_Raw()
        {
            var elementTitles = new[] { "Home", "About", "Contact Us", "Portfolio", "Gallery", "AndThisOneNeverEverExists" };

            // TODO: Use a Probe for Exists, Displayed, Enabled directly
            foreach (var elementTitle in elementTitles)
            {
                var exists = Browser.FindElements(By.XPath($"//a[text()='{elementTitle}']"));
                Console.WriteLine($"The element title {elementTitle} exists: {exists.Count() != 0}");
            }
        }
    }
}
