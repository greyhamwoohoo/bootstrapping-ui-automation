using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Linq;
using TheInternet.Common.Reporting.Contracts;

namespace TheInternet.Common.Reporting
{
    /// <summary>
    /// Ensures the ReportingContexts are consistently configured in the service collection. 
    /// </summary>
    internal class ReportingContextRegistrationManager
    {
        private readonly ILogger _bootstrappingLogger;
        private readonly IServiceCollection _services;
        private readonly ITestRunReporterContext _testRunReporterContext;
        private readonly bool _testRunReporter;
        private readonly bool _testCaseReporterContext;
        private readonly bool _testCaseReporter;

        internal ReportingContextRegistrationManager(ILogger bootstrappingLogger, IServiceCollection services, ITestRunReporterContext testRunReporterContext)
        {
            _bootstrappingLogger = bootstrappingLogger ?? throw new System.ArgumentNullException(nameof(bootstrappingLogger));
            _services = services ?? throw new System.ArgumentNullException(nameof(services));
            _testRunReporterContext = testRunReporterContext ?? throw new System.ArgumentNullException(nameof(testRunReporterContext));

            _testRunReporter = services.Any(x => x.ServiceType == typeof(ITestRunReporter));
            _testCaseReporterContext = services.Any(x => x.ServiceType == typeof(ITestCaseReporterContext));
            _testCaseReporter = services.Any(x => x.ServiceType == typeof(ITestCaseReporter));
        }

        internal bool IsConfigured => _testRunReporter && _testCaseReporterContext && _testCaseReporter;
        internal bool IsPartiallyConfigured => !IsConfigured && (_testRunReporter || _testCaseReporterContext || _testCaseReporter);
        internal void AssertIsNotPartiallyConfigured()
        {
            if (IsPartiallyConfigured)
            {
                throw new System.InvalidOperationException($"The ReportingContext is not correctly configured. {typeof(ITestRunReporter).FullName},{typeof(ITestCaseReporterContext).FullName} and {typeof(ITestCaseReporter).FullName} must be registered together; or not at all. ");
            }
        }
        internal void PopulateDefaultReportingContexts()
        {
            _services.AddSingleton<ITestRunReporter>(isp =>
            {
                return new TestRunReporter(_bootstrappingLogger, testOutputFolder: _testRunReporterContext.RootReportingFolder, _testRunReporterContext.TestRunIdentity);
            });

            _services.AddScoped<ITestCaseReporterContext>(isp =>
            {
                var testRunReporter = isp.GetRequiredService<ITestRunReporter>();

                return new TestCaseReporterContext(rootOutputFolder: testRunReporter.ReportOutputFolder, testCaseIdentity: $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}");
            });

            _services.AddScoped<ITestCaseReporter>(isp =>
            {
                var testRunReporter = isp.GetRequiredService<ITestRunReporter>();
                var testCaseReporterContext = isp.GetRequiredService<ITestCaseReporterContext>();
                var logger = isp.GetRequiredService<ILogger>();

                return testRunReporter.CreateTestCaseReporter(logger, testCaseReporterContext);
            });
        }
    }
}
