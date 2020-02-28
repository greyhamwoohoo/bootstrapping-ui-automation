using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheInternet.Common.ExecutionContext.Runtime.BrowserSettings.Contracts;

namespace TheInternet.SystemTests.Raw
{
    [TestClass]
    public class SmokeTests : SeleniumTestBase
    {
        [TestMethod]
        public void ItIsChrome()
        {
            Resolve<IBrowserProperties>().Name.Should().BeOneOf("CHROME", "EDGE");
        }

        [TestMethod]
        public void BrowserSessionExists()
        {
            BrowserSession.Should().NotBeNull(because: "the BrowserSession is instantiated in the Base Class. ");
        }
    }
}
