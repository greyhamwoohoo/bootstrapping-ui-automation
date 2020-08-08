using AventStack.ExtentReports;
using TheInternet.Common.Reporting.Contracts;

namespace TheInternet.Common.ExtentReports
{
    public class TestCaseReporter : ITestCaseReporter
    {
        public string Name { get; private set; }
        public string LogFilePath { get; }
        public ITestRunReporter TestRunReporter => ExtentTestRunReporter;
        private TestRunReporter ExtentTestRunReporter { get; set; }
        private ExtentTest _extentTest;
        public TestCaseReporter(TestRunReporter testRunReporter)
        {
            ExtentTestRunReporter = testRunReporter ?? throw new System.ArgumentNullException(nameof(testRunReporter));
        }

        public void Initialize(string name)
        {
            if (_extentTest != null) throw new System.InvalidOperationException($"The TestCaseReporter is already initialized. ");

            Name = name ?? throw new System.ArgumentNullException(nameof(name));

            _extentTest = ExtentTestRunReporter.ExtentReporter.CreateTest(name);
        }

        public void Uninitialize()
        {
            _extentTest = null;
        }
    }
}
