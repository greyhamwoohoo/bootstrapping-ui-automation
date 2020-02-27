using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheInternet.Common.ExecutionContext.Runtime.BrowserSettings;
using TheInternet.Common.ExecutionContext.Runtime.BrowserSettings.Contracts;

namespace TheInternet.SystemTests.Raw
{
    [TestClass]
    public class SmokeTests : TestBase
    {
        [TestMethod]
        public void ItIsChrome()
        {
            var settings = Resolve<ChromeBrowserSettings>();
            var browserName = Resolve<IBrowserName>().Value;
        }
    }
}
