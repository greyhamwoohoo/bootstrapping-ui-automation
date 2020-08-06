namespace TheInternet.Common.Infrastructure.Contracts
{
    /// <summary>
    /// Describes the test run context such as deployment folder and so forth. 
    /// </summary>
    public interface ITestRunContext
    {
        /// <summary>
        /// Prefix used for environment variables and uniquely identify 
        /// </summary>
        string Prefix { get; }
        /// <summary>
        /// The TestExecutionContext currently active
        /// </summary>
        string TextExecutionContext { get; }
    }
}
