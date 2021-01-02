﻿using AventStack.ExtentReports;
using OpenQA.Selenium;
using Serilog;
using System;
using TheInternet.Common.Reporting.Contracts;
using TheInternet.Common.SessionManagement.Contracts;

namespace TheInternet.Common.ExtentReports
{
    public class TestCaseReporter : ITestCaseReporter
    {
        public string Name { get; private set; }
        public string LogFilePath { get; }
        public ITestRunReporter TestRunReporter => ExtentTestRunReporter;
        public ILogger Logger { get; }
        public bool AlwaysCaptureScreenshots { get; set; }
        public IDriverSession DriverSession { get; private set; }
        public string RootOutputFolder => System.IO.Path.GetDirectoryName(LogFilePath);
        private TestRunReporter ExtentTestRunReporter { get; set; }
        private ExtentTest _extentTest;
        public TestCaseReporter(ILogger logger, TestRunReporter testRunReporter, string logFilePath)
        {
            Logger = logger ?? throw new System.ArgumentNullException(nameof(testRunReporter));
            ExtentTestRunReporter = testRunReporter ?? throw new System.ArgumentNullException(nameof(testRunReporter));
            LogFilePath = logFilePath ?? throw new System.ArgumentNullException(nameof(logFilePath));
        }

        public void Initialize(IDriverSession driverSession, string name)
        {
            if (_extentTest != null) throw new System.InvalidOperationException($"The TestCaseReporter is already initialized. ");

            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            DriverSession = driverSession ?? throw new ArgumentNullException(nameof(driverSession));

            _extentTest = ExtentTestRunReporter.ExtentReporter.CreateTest(name);
        }

        public void Uninitialize()
        {
            _extentTest = null;
        }

        public void Debug(string message)
        {
            Log(Status.Debug, $"{message}");
        }

        public void Information(string message)
        {
            Log(Status.Info, $"{message}");
        }

        public void Warning(string message)
        {
            Log(Status.Warning, $"{message}");
        }

        public void Error(string message)
        {
            Log(Status.Error, $"{message}");
        }

        public void Error(string message, Exception exception)
        {
            Log(Status.Error, $"{message}<br/>{exception}");
        }


        public void Pass(string message)
        {
            Log(Status.Pass, message);
        }

        public void Fail(string message)
        {
            Log(Status.Fail, message);
        }

        public void Fail(string message, Exception exception)
        {
            Log(Status.Fail, $"{message}<br/>{exception}");
        }

        private void Log(Status status, string message)
        {
            AssertDriverSession();

            Logger.Debug($"ExtentReporter:Write{status},{message}");

            if(AlwaysCaptureScreenshots)
            {
                var screenshot = ((ITakesScreenshot)DriverSession.WebDriver).GetScreenshot();

                var screenshotsFolder = System.IO.Path.Combine(RootOutputFolder, "Screenshots");
                System.IO.Directory.CreateDirectory(screenshotsFolder);
                var filename = System.IO.Path.Combine(screenshotsFolder, DateTime.Now.ToString("HHmmssfffff"));

                screenshot.SaveAsFile(filename);

                _extentTest.Log(status, message, MediaEntityBuilder.CreateScreenCaptureFromPath(filename).Build());
            }
            else
            {
                _extentTest.Log(status, message);
            }
        }

        private void AssertDriverSession()
        {
            if (DriverSession == null) throw new System.InvalidOperationException($"You must call Bind() to pass in the IDriverSession");
        }
    }
}
