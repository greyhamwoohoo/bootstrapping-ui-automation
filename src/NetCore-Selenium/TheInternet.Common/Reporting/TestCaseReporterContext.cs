using TheInternet.Common.Reporting.Contracts;

namespace TheInternet.Common.Reporting
{
    public class TestCaseReporterContext : ITestCaseReporterContext
    {
        public TestCaseReporterContext(string logPath)
        {
            LogPath = logPath ?? throw new System.ArgumentNullException(nameof(logPath));
        }

        public string LogPath { get; }
    }
}
