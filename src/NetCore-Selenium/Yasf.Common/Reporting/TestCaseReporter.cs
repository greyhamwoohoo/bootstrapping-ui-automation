using Serilog;
using System;
using Yasf.Common.Reporting.Contracts;
using Yasf.Common.SessionManagement.Contracts;

namespace Yasf.Common.Reporting
{
    public class TestCaseReporter : ITestCaseReporter
    {
        public string Name { get; private set; }
        public string LogFilePath { get; }
        protected ILogger Logger { get; }
        public bool AlwaysCaptureScreenshots { get; set; }
        public IDriverSession DriverSession { get; private set; }

        public TestCaseReporter(ILogger logger, ITestCaseReporterContext testCaseReporterContext)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            if (null == testCaseReporterContext) throw new ArgumentNullException(nameof(testCaseReporterContext));

            LogFilePath = testCaseReporterContext.LogFilePath;
        }

        public void Initialize(IDriverSession driverSession, string name)
        {
            DriverSession = driverSession ?? throw new ArgumentNullException(nameof(driverSession));
            Name = name ?? throw new ArgumentNullException(nameof(name));

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

        public void Pass(string message)
        {
            Logger.Information($"Pass: {message}");
        }

        public void Fail(string message)
        {
            Logger.Error($"Fail: {message}");
        }

        public void Fail(string message, Exception exception)
        {
            Logger.Error($"Fail: {message}\r\n{exception}");
        }
    }
}
