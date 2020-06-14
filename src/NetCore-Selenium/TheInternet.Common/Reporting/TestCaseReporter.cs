using Serilog;
using TheInternet.Common.Reporting.Contracts;

namespace TheInternet.Common.Reporting
{
    public class TestCaseReporter : ITestCaseReporter
    {
        protected ILogger Logger { get; }
        public string Name { get; private set; }
        public string LogPath { get; }

        public TestCaseReporter(ILogger logger, string logPath)
        {
            Logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            LogPath = logPath ?? throw new System.ArgumentNullException(nameof(logPath));
        }

        public void Initialize(string name)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));

            Logger.Debug($"Initialize: Name={name}");
        }

        public void Uninitialize()
        {
            Name = null;
            Logger.Debug($"Uninitialize");
        }
    }
}
