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
        /// Log path for the current test case. All Logger calls are written to this file. 
        /// </summary>
        string LogPath { get; }
    }
}
