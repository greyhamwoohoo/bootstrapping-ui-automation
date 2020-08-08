using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheInternet.Common;
using TheInternet.Common.ExecutionContext.Runtime.BrowserSettings.Contracts;

namespace TheInternet.SystemTests.Raw
{
    [TestClass]
    public class SmokeTest : SeleniumTestBase
    {
        protected override string BaseUrl => "https://www.google.com";

        [TestMethod]
        public void ItIsChrome()
        {
            Resolve<IBrowserProperties>().Name.Should().BeOneOf("CHROME", "EDGE", "FIREFOX");
        }
    }
}
