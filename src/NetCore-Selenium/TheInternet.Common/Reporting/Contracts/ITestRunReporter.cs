namespace TheInternet.Common.Reporting.Contracts
{
    public interface ITestRunReporter
    {
        /// <summary>
        /// Folder where the test DLLs reside
        /// </summary>
        string TestDeploymentFolder { get; }

        /// <summary>
        /// Called once by the Test Engine to initialize the test run.
        /// </summary>
        void Setup();

        /// <summary>
        /// Called once by the Test Engine to uninitialize the test run.
        /// </summary>
        void Teardown();
    }
}
