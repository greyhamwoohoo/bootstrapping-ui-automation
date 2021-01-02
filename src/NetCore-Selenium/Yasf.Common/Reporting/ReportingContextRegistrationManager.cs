using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Yasf.Common.Reporting.Contracts;

[assembly: InternalsVisibleTo("Yasf.Common.UnitTests")]
namespace Yasf.Common.Reporting
{
    /// <summary>
    /// Ensures the ReportingContexts are consistently configured in the service collection. 
    /// </summary>
    internal class ReportingContextRegistrationManager
    {
        private readonly ILogger _bootstrappingLogger;
        private readonly IServiceCollection _services;
        private readonly ITestRunReporterContext _testRunReporterContext;
        private bool _testRunReporter;
        private bool _testCaseReporterContext;
        private bool _testCaseReporter;

        internal ReportingContextRegistrationManager(ILogger bootstrappingLogger, IServiceCollection services, ITestRunReporterContext testRunReporterContext)
        {
            _bootstrappingLogger = bootstrappingLogger ?? throw new ArgumentNullException(nameof(bootstrappingLogger));
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _testRunReporterContext = testRunReporterContext ?? throw new ArgumentNullException(nameof(testRunReporterContext));

            RefreshRegistrationState();
        }

        internal bool IsConfigured => _testRunReporter && _testCaseReporterContext && _testCaseReporter;
        internal bool IsPartiallyConfigured => !IsConfigured && (_testRunReporter || _testCaseReporterContext || _testCaseReporter);
        internal void AssertIsNotPartiallyConfigured()
        {
            if (IsPartiallyConfigured)
            {
                throw new InvalidOperationException($"The ReportingContext is not correctly configured. {typeof(ITestRunReporter).FullName},{typeof(ITestCaseReporterContext).FullName} and {typeof(ITestCaseReporter).FullName} must be registered together; or not at all. ");
            }
        }

        internal void PopulateDefaultReportingContexts()
        {
            if (IsConfigured || IsPartiallyConfigured)
            {
                throw new InvalidOperationException($"The Reporting Contexts are already configured. ");
            }

            _services.AddSingleton<ITestRunReporter>(isp =>
            {
                return new TestRunReporter(_bootstrappingLogger, testOutputFolder: _testRunReporterContext.RootReportingFolder, _testRunReporterContext.TestRunIdentity);
            });

            _services.AddScoped<ITestCaseReporterContext>(isp =>
            {
                var testRunReporter = isp.GetRequiredService<ITestRunReporter>();

                return new TestCaseReporterContext(rootOutputFolder: testRunReporter.ReportOutputFolder, testCaseIdentity: $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}");
            });

            _services.AddScoped(isp =>
            {
                var testRunReporter = isp.GetRequiredService<ITestRunReporter>();
                var testCaseReporterContext = isp.GetRequiredService<ITestCaseReporterContext>();
                var logger = isp.GetRequiredService<ILogger>();

                return testRunReporter.CreateTestCaseReporter(logger, testCaseReporterContext);
            });

            RefreshRegistrationState();
        }

        private void RefreshRegistrationState()
        {
            _testRunReporter = _services.Where(x => x.ServiceType == typeof(ITestRunReporter)).Count() == 1;
            _testCaseReporterContext = _services.Where(x => x.ServiceType == typeof(ITestCaseReporterContext)).Count() == 1;
            _testCaseReporter = _services.Where(x => x.ServiceType == typeof(ITestCaseReporter)).Count() == 1;
        }
    }
}
