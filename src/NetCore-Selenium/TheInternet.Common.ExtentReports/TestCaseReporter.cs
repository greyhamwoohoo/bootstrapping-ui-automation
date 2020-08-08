using AventStack.ExtentReports;
using Serilog;
using System;
using TheInternet.Common.Reporting.Contracts;

namespace TheInternet.Common.ExtentReports
{
    public class TestCaseReporter : ITestCaseReporter
    {
        public string Name { get; private set; }
        public string LogFilePath { get; }
        public ITestRunReporter TestRunReporter => ExtentTestRunReporter;

        public ILogger Logger { get; }
        private TestRunReporter ExtentTestRunReporter { get; set; }
        private ExtentTest _extentTest;
        public TestCaseReporter(ILogger logger, TestRunReporter testRunReporter)
        {
            Logger = logger ?? throw new System.ArgumentNullException(nameof(testRunReporter));
            ExtentTestRunReporter = testRunReporter ?? throw new System.ArgumentNullException(nameof(testRunReporter));
        }

        public void Initialize(string name)
        {
            if (_extentTest != null) throw new System.InvalidOperationException($"The TestCaseReporter is already initialized. ");

            Name = name ?? throw new System.ArgumentNullException(nameof(name));

            _extentTest = ExtentTestRunReporter.ExtentReporter.CreateTest(name);
        }

        public void Uninitialize()
        {
            _extentTest = null;
        }

        public void Debug(string message)
        {
            Logger.Debug($"{message}");
            _extentTest.Debug($"{message}");
        }

        public void Information(string message)
        {
            Logger.Information($"{message}");
            _extentTest.Info($"{message}");
        }

        public void Warning(string message)
        {
            Logger.Warning($"{message}");
            _extentTest.Warning($"{message}");
        }

        public void Error(string message)
        {
            Logger.Error($"{message}");
            _extentTest.Error($"{message}");
        }

        public void Error(string message, Exception exception)
        {
            Logger.Error($"{exception}");
            _extentTest.Error($"{exception}");
        }
    }
}
