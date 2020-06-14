using Serilog;
using TheInternet.Common.Reporting.Contracts;

namespace TheInternet.Common.Reporting
{
    public class TestCaseReporter : ITestCaseReporter
    {
        public string Name { get; private set; }
        protected ILogger Logger { get; }
        public TestCaseReporter(ILogger logger)
        {
            Logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public void Initialize(string name)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));

            Logger.Debug($"Initialize: Name={name}");
        }

        public void Uninitialize()
        {
            Name = null;
            Logger.Debug($"Uninitialize:");
        }
    }
}
