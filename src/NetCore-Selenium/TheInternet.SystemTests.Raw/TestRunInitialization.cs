using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TheInternet.Common.Infrastructure;
using TheInternet.SystemTests.Raw.Infrastructure;

namespace TheInternet.SystemTests.Raw
{
    [TestClass]
    public class TestRunInitialization
    {
        const string PREFIX = "THEINTERNET_";

        public const string DEFAULT_TEST_EXECUTION_CONTEXT = "default-chrome-localhost";
        public const string TEST_EXECUTION_CONTEXT_KEY_NAME = "TestExecutionContext";

        static IServiceProvider _serviceProvider = default;

        [AssemblyInitialize]
        public static void Initialize(TestContext testContext)
        {
            // The Text Execution Context (environment, variables) are chosen in this order:
            //
            // 1. {PREFIX}TEST_EXECUTION_CONTEXT
            // 2. DEFAULT_TEST_EXECUTION_CONTEXT
            // 3. .runsettings is the fallback (if in the solution root)
            var testExecutionContextName = $"{Environment.GetEnvironmentVariable($"{PREFIX}TEST_EXECUTION_CONTEXT") ?? DEFAULT_TEST_EXECUTION_CONTEXT}";
            if (testContext.Properties.Contains(TEST_EXECUTION_CONTEXT_KEY_NAME))
            {
                testExecutionContextName = Convert.ToString(testContext.Properties[TEST_EXECUTION_CONTEXT_KEY_NAME]);
            }

            var testExecutionContextFilename = $"tec.{testExecutionContextName}.json";

            var testExecutionContext = TestExecutionContextFactory.Create(testExecutionContextFilename);

            var environmentVariables = testExecutionContext.EnvironmentVariables;
            environmentVariables.Keys.ToList().ForEach(ev =>
            {
                Environment.SetEnvironmentVariable(ev, environmentVariables[ev], EnvironmentVariableTarget.Process);
            });

            ContainerSingleton.Initialize(PREFIX, (prefix, services) =>
            {
                services.AddSingleton(testExecutionContext);
            });

            _serviceProvider = ContainerSingleton.Instance;
        }

        [AssemblyCleanup]
        public static void Teardown()
        {

        }
    }
}
