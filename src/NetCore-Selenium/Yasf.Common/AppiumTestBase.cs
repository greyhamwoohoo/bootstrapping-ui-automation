
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yasf.Common
{
    [TestClass]
    public abstract class AppiumTestBase : SeleniumTestBase
    {
        public AppiumTestBase()
        {
        }

        protected override void NavigateToBaseUrl()
        {
            // Do nothing - do not apply to Appium by default
        }
    }
}
