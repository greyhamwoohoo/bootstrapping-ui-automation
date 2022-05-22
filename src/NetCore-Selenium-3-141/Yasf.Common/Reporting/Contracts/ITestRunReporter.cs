using Serilog;

namespace Yasf.Common.Reporting.Contracts
{
    public interface ITestRunReporter
    {
        /// <summary>
        /// The top level folder where test results are to be written.
        /// </summary>
        string RootOutputFolder { get; }

        /// <summary>
        /// Unique identitiy for this test run - typically combined by the report with TestOutputFolder to group results. 
        /// </summary>
        string TestRunIdentity { get; }

        /// <summary>
        /// The top level folder containing reports for this test run are to be written. ie: this is a combination of RootOutputFolder and TestRunIdentity
        /// </summary>
        string ReportOutputFolder { get; }

        /// <summary>
        /// Called once by the Test Engine to initialize the test run.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Called once by the Test Engine to uninitialize the test run.
        /// </summary>
        void Uninitialize();

        /// <summary>
        /// Create a new Test Case reporter. 
        /// </summary>
        /// <param name="logger">logger to use</param>
        /// <param name="testCaseReporterContext">The reporting context for this test case</param>
        /// <returns></returns>
        ITestCaseReporter CreateTestCaseReporter(ILogger logger, ITestCaseReporterContext testCaseReporterContext);
    }
}
