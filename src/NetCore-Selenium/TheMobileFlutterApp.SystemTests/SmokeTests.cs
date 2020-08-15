using FluentAssertions;
using GreyhamWooHoo.Flutter.Finder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheMobileFlutterApp.SystemTests
{
    /// <summary>
    /// The Test App (and system tests) can be found at this page:
    /// https://github.com/greyhamwoohoo/appium-flutter-driver-net-bindings
    /// </summary>
    [TestClass]
    public class SmokeTests : FlutterMobileTestBase
    {
        [TestMethod]
        public void NavigateToTextTestPage()
        {
            FlutterDriver.Click(FlutterBy.Text("Navigate to Taps Test Page"));

            FlutterDriver.WaitFor(FlutterBy.Text("Taps Page"));

            FlutterDriver.GetText(FlutterBy.ValueKey("tapCounter")).Should().Be("0", because: "the counter is initially 0");
        }
    }
}
