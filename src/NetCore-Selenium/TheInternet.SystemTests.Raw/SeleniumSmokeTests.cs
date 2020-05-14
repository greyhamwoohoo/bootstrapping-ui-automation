using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using TheInternet.Common.ExecutionContext.Runtime.BrowserSettings.Contracts;
using TheInternet.Common.ExecutionContext.Runtime.ControlSettings;
using TheInternet.Common.ExecutionContext.Runtime.InstrumentationSettings;

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

        [TestMethod]
        public void InstrumentationSettingsExists()
        {
            var instrumentationSettingsExists = Resolve<IInstrumentationSettings>();
            var controlSettings = Resolve<IControlSettings>();
        }
    }
}
