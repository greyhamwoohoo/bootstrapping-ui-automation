using TheInternet.Common.ExecutionContext.Runtime.InstrumentationSettings;

namespace TheInternet.Common.Reporting.Contracts
{
    /// <summary>
    /// Context for the entire test run. 
    /// </summary>
    public interface ITestRunReporterContext
    {
        /// <summary>
        /// The top level folder where test results are to be written.
        /// </summary>
        string RootReportingFolder { get; }

        /// <summary>
        /// Unique identitiy for this test run - typically combined by the report with TestOutputFolder to group results. 
        /// </summary>
        string TestRunIdentity { get; }
        /// <summary>
        /// Instrumentation settings for this test run. 
        /// </summary>
        IInstrumentationSettings InstrumentationSettings { get; }
    }
}
