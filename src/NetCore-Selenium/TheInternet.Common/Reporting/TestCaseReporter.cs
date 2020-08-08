using Serilog;
using System;
using TheInternet.Common.Reporting.Contracts;

namespace TheInternet.Common.Reporting
{
    public class TestCaseReporter : ITestCaseReporter
    {
        public string Name { get; private set; }
        public string LogFilePath { get; }
        public ITestRunReporter TestRunReporter { get; }
        protected ILogger Logger { get; }

        internal TestCaseReporter(ITestRunReporter testRunReporter, ILogger logger, ITestCaseReporterContext testCaseReporterContext)
        {
            TestRunReporter = testRunReporter ?? throw new System.ArgumentNullException(nameof(testRunReporter));
            Logger = logger ?? throw new System.ArgumentNullException(nameof(logger));

            LogFilePath = testCaseReporterContext.LogFilePath;
        }

        public void Initialize(string name)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));

            Logger.Debug($"Initialize: Name={name}");
        }

        public void Uninitialize()
        {
            Name = null;
            Logger.Debug($"Uninitialize");
        }

        public void Debug(string message)
        {
            Logger.Debug($"{message}");
        }

        public void Information(string message)
        {
            Logger.Information($"{message}");
        }

        public void Warning(string message)
        {
            Logger.Warning($"{message}");
        }

        public void Error(string message)
        {
            Logger.Error($"{message}");
        }

        public void Error(string message, Exception exception)
        {
            Logger.Error($"{exception}");
        }
    }
}
