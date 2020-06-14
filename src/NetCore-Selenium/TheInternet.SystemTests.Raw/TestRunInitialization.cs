using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using System;
using System.Linq;
using TheInternet.Common.Infrastructure;
using TheInternet.Common.Reporting.Contracts;
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
        static ITestRunReporter _testRunReporter = default;

        [AssemblyInitialize]
        public static void Initialize(TestContext testContext)
        {
            // Initialize this early on (for bootstrapping) and keep it simple: console output only (MsTest will capture this)
            Log.Logger = new LoggerConfiguration()
                        .Enrich
                        .FromLogContext()
                        .WriteTo
                        .Console()
                        .CreateLogger();

            // The Text Execution Context (environment, variables) are chosen in this order:
            //
            // 1. {PREFIX}TEST_EXECUTION_CONTEXT
            // 2. DEFAULT_TEST_EXECUTION_CONTEXT
            // 3. .runsettings is the fallback (if in the solution root)
            var testExecutionContextName = $"{Environment.GetEnvironmentVariable($"{PREFIX}TEST_EXECUTION_CONTEXT") ?? DEFAULT_TEST_EXECUTION_CONTEXT}";
            Log.Logger.Information($"Candidate Test Exection Context to use: {testExecutionContextName}");

            if (testContext.Properties.Contains(TEST_EXECUTION_CONTEXT_KEY_NAME))
            {
                Log.Logger.Information($"The .runsettings contains a property called {TEST_EXECUTION_CONTEXT_KEY_NAME}. We will retrieve that. ");
                testExecutionContextName = Convert.ToString(testContext.Properties[TEST_EXECUTION_CONTEXT_KEY_NAME]);
            }

            Log.Logger.Information($"TestExecutionContextName to use: {testExecutionContextName}");

            var testExecutionContextFilename = $"tec.{testExecutionContextName}.json";
            Log.Logger.Information($"TestExecutionContextFilename to be used: {testExecutionContextFilename}");

            var testExecutionContext = TestExecutionContextFactory.Create(testExecutionContextFilename);

            var environmentVariables = testExecutionContext.EnvironmentVariables;
            environmentVariables.Keys.ToList().ForEach(ev =>
            {
                Log.Logger.Information($"Setting Environment Variable '{ev}' to '{environmentVariables[ev]}' for this process. ");

                Environment.SetEnvironmentVariable(ev, environmentVariables[ev], EnvironmentVariableTarget.Process);
            });

            Log.Logger.Information("START: To initialize singleton container. ");
            ContainerSingleton.Initialize(Log.Logger, PREFIX, (prefix, services) =>
            {
                Log.Logger.Information($"    START: Callback before container is built. ");
                services.AddSingleton(testExecutionContext);
                Log.Logger.Information($"    END: Callback after container is built. ");
            });
            Log.Logger.Information("END: Initializing singleton container. ");

            _serviceProvider = ContainerSingleton.Instance;

            _testRunReporter = _serviceProvider.GetRequiredService<ITestRunReporter>();
            _testRunReporter.Setup();
        }

        [AssemblyCleanup]
        public static void Teardown()
        {
            _testRunReporter.Teardown();
        }
    }
}
