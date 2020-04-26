﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using TheInternet.Common.ExecutionContext.Runtime.BrowserSettings.Contracts;

namespace TheInternet.SystemTests.Raw
{
    [TestClass]
    public class SeleniumSmokeTests : SeleniumTestBase
    {
        protected override string BaseUrl => "https://www.google.com";

        [TestMethod]
        public void ItIsChrome()
        {
            Resolve<IBrowserProperties>().Name.Should().BeOneOf("CHROME", "EDGE", "FIREFOX");
        }

        [TestMethod]
        public void DriverSessionExists()
        {
            Logger.Information($"Here I am");
            DriverSession.Should().NotBeNull(because: "the DriverSession is instantiated in the Base Class. ");
            Logger.Information($"Here I am again");
        }
    }
}
