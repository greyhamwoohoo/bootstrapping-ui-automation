using System;
using TheInternet.Common.SessionManagement;
using TheInternet.Common.SessionManagement.Contracts;

namespace TheInternet.Common.Reporting.Contracts
{
    /// <summary>
    /// Represents the running test case. 
    /// </summary>
    public interface ITestCaseReporter
    {
        /// <summary>
        /// Initialize the reporter for the given test. 
        /// </summary>
        /// <param name="name">Test name</param>
        /// <param name="driverSession">The driver session this TestCase belongs to. </param>
        void Initialize(IDriverSession driverSession, string name);

        /// <summary>
        /// Uninitialize / tear down. 
        /// </summary>
        void Uninitialize();

        /// <summary>
        /// Name of the current test case. 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Fully qualified path to the .log file that will be created for this test case. 
        /// </summary>
        string LogFilePath { get; }
        bool AlwaysCaptureScreenshots { get; set; }
        IDriverSession DriverSession { get; }
        void Debug(string message);
        void Information(string message);
        void Warning(string message);
        void Error(string message);
        void Error(string message, Exception ex);
    }
}
