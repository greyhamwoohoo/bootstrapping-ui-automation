using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using System;
using TheInternet.Common.Infrastructure;
using TheInternet.Common.Reporting;
using TheInternet.Common.Reporting.Contracts;

namespace TheInternet.Common.ExtentReports.SystemTests.Infrastructure
{
    [TestClass]
    public class TestRunInitialization
    {
        const string PREFIX = "THEINTERNET_";

        public const string DEFAULT_TEST_EXECUTION_CONTEXT = "default-chrome-localhost";

        [AssemblyInitialize]
        public static void Initialize(TestContext testContext)
        {
            MsTestInitialization.Initialize(PREFIX, DEFAULT_TEST_EXECUTION_CONTEXT, testContext, RegisterExtentReporting);
        }

        [AssemblyCleanup]
        public static void Teardown()
        {
            MsTestInitialization.Uninitialize();
        }

        private static void RegisterExtentReporting(string prefix, IServiceCollection serviceCollection, ITestRunReporterContext testRunReporterContext)
        {
            if(!testRunReporterContext.InstrumentationSettings.ReportingSettings.Enabled)
            {
                return;
            }

            var testRunReporter = new TestRunReporter(testRunReporterContext.RootReportingFolder, testRunReporterContext.TestRunIdentity);

            serviceCollection.AddSingleton<ITestRunReporter>(isp =>
            {
                return testRunReporter;
            });

            serviceCollection.AddScoped<ITestCaseReporterContext>(isp =>
            {
                return new TestCaseReporterContext(rootOutputFolder: testRunReporter.ReportOutputFolder, testCaseIdentity: $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}");
            });

            serviceCollection.AddScoped(isp =>
            {
                var testRunReporter = isp.GetRequiredService<ITestRunReporter>();
                var testCaseReporterContext = isp.GetRequiredService<ITestCaseReporterContext>();
                var logger = isp.GetRequiredService<ILogger>();

                var result = testRunReporter.CreateTestCaseReporter(logger, testCaseReporterContext);
                result.AlwaysCaptureScreenshots = testRunReporterContext.InstrumentationSettings.ReportingSettings.AlwaysCaptureScreenshots;
                return result;
            });
        }
    }
}
