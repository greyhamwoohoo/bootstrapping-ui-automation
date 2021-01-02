using Serilog;

namespace TheInternet.Common.Reporting.Contracts
{
    public interface ITestCaseReporterContext
    {
        /// <summary>
        /// Fully qualified path to the .log file that will be created for this test case. 
        /// </summary>
        string LogFilePath { get; }
        /// <summary>
        /// The identity to use for this test case. Typically: a time stamp, to uniquely identify filenames and folder to be used for reporting. 
        /// </summary>
        string TestCaseIdentity { get; }
    }
}
