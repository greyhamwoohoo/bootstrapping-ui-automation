using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheInternet.Common.Infrastructure;

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
            MsTestInitialization.Initialize(PREFIX, DEFAULT_TEST_EXECUTION_CONTEXT, testContext, PopulateContainer);
        }

        [AssemblyCleanup]
        public static void Teardown()
        {
            MsTestInitialization.Uninitialize();
        }

        private static void PopulateContainer(string prefix, IServiceCollection serviceCollection)
        {
        }
    }
}
