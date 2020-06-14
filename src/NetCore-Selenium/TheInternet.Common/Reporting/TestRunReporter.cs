using Serilog;
using TheInternet.Common.Reporting.Contracts;

namespace TheInternet.Common.Reporting
{
    public class TestRunReporter : ITestRunReporter
    {
        public string TestDeploymentFolder { get; }
        private bool _initialized { get; set;  }
        private ILogger _logger { get; }

        public TestRunReporter(ILogger logger, string testDeploymentFolder)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            TestDeploymentFolder = testDeploymentFolder ?? throw new System.ArgumentNullException(nameof(testDeploymentFolder));
        }

        public void Setup()
        {
            _logger.Debug($"Setup: Existing state: {_initialized}");
            if (_initialized) return;

            _initialized = true;
            
            _logger.Debug($"Setup: New state: {_initialized}");
        }

        public void Teardown()
        {
            _logger.Debug($"Teardown: Existing state: {_initialized}");
         
            if (!_initialized) return;

            _initialized = false;

            _logger.Debug($"Teardown: New state: {_initialized}");
        }
    }
}
