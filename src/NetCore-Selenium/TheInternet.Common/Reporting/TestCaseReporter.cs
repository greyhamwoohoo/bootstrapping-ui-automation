using Serilog;
using System;
using TheInternet.Common.Reporting.Contracts;
using TheInternet.Common.SessionManagement.Contracts;

namespace TheInternet.Common.Reporting
{
    public class TestCaseReporter : ITestCaseReporter
    {
        public string Name { get; private set; }
        public string LogFilePath { get; }
        protected ILogger Logger { get; }
        public bool AlwaysCaptureScreenshots { get; set; }
        public IDriverSession DriverSession { get; private set; }

        internal TestCaseReporter(ITestRunReporter testRunReporter, ILogger logger, ITestCaseReporterContext testCaseReporterContext)
        {
            Logger = logger ?? throw new System.ArgumentNullException(nameof(logger));

            LogFilePath = testCaseReporterContext.LogFilePath;
        }

        public void Initialize(IDriverSession driverSession, string name)
        {
            DriverSession = driverSession ?? throw new System.ArgumentNullException(nameof(driverSession));
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
