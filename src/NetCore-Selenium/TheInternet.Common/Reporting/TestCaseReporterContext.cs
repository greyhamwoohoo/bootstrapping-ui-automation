using TheInternet.Common.Reporting.Contracts;

namespace TheInternet.Common.Reporting
{
    public class TestCaseReporterContext : ITestCaseReporterContext
    {
        public TestCaseReporterContext(string rootOutputFolder, string testCaseIdentity)
        {
            if (rootOutputFolder == null) throw new System.ArgumentNullException(nameof(rootOutputFolder));
            if (testCaseIdentity == null) throw new System.ArgumentNullException(nameof(testCaseIdentity));

            LogFilePath = System.IO.Path.Combine(rootOutputFolder, $"{testCaseIdentity}.log");
            TestCaseIdentity = testCaseIdentity;
        }

        public string LogFilePath { get; }

        public string TestCaseIdentity { get; }
    }
}
