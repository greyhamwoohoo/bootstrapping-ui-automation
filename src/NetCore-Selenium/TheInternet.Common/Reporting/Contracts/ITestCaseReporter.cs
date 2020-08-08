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
        void Initialize(string name);

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
        /// <summary>
        /// Reporter used for the entire test run (not just this test case)
        /// </summary>
        ITestRunReporter TestRunReporter { get; }
    }
}
