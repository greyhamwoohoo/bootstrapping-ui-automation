using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yasf.Common.Infrastructure;
using Yasf.Common.Reporting.Contracts;

namespace TheInternet.SystemTests.Raw.Infrastructure
{
    [TestClass]
    public class TestRunInitialization
    {
        const string PREFIX = "THEINTERNET_";

        public const string DEFAULT_TEST_EXECUTION_CONTEXT = "default-chrome-localhost";

        [AssemblyInitialize]
        public static void Initialize(TestContext testContext)
        {
            MsTestInitialization.Initialize(PREFIX, DEFAULT_TEST_EXECUTION_CONTEXT, testContext, UseDefaultReportingContexts);
        }

        [AssemblyCleanup]
        public static void Teardown()
        {
            MsTestInitialization.Uninitialize();
        }

        private static void UseDefaultReportingContexts(string prefix, IServiceCollection serviceCollection, ITestRunReporterContext testRunReporterContext)
        {
            // By doing nothing, the container will be populated with the default services. 
        }
    }
}
