using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheInternet.Common.ExecutionContext.Runtime.BrowserSettings.Contracts;
using TheInternet.Common.ExecutionContext.Runtime.ControlSettings;
using TheInternet.Common.ExecutionContext.Runtime.InstrumentationSettings;
using TheInternet.Common.Reporting.Contracts;

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

            DriverSession.TestCaseReporter.Name.Should().Be(TestContext.TestName, because: "the test case should have been initialized. ");
        }

        [TestMethod]
        public void InstrumentationSettingsExists()
        {
            var instrumentationSettingsExists = Resolve<IInstrumentationSettings>();
            var controlSettings = Resolve<IControlSettings>();
        }

        [TestMethod]
        public void ResolutionOfSomethings()
        {
            var folder = System.IO.Path.GetDirectoryName(typeof(SeleniumSmokeTests).Assembly.Location);

            Resolve<ITestRunReporter>().TestDeploymentFolder.Should().Be(folder, because: "the test deployment folder is where the tests are deployed. ");
        }
    }
}
