using Yasf.Common.ElementOperations.Contracts;
using Yasf.Common.ExecutionContext.Runtime.ControlSettings;
using Yasf.Common.ExecutionContext.Runtime.EnvironmentSettings;
using Yasf.Common.Reporting.Contracts;
using Yasf.Common.SessionManagement.Contracts;

namespace Yasf.Common.SessionManagement
{
    public class DriverSession : IDriverSession
    {
        public IDecoratedWebDriver WebDriver { get; }
        public EnvironmentSettings EnvironmentSettings { get; }
        public IControlSettings ControlSettings { get; }
        public ITestCaseReporter TestCaseReporter { get; }

        public DriverSession(IDecoratedWebDriver webDriver, EnvironmentSettings environmentSettings, IControlSettings controlSettings, ITestCaseReporter testCaseReporter)
        {
            if (webDriver == null) throw new System.ArgumentNullException(nameof(webDriver));
            if (environmentSettings == null) throw new System.ArgumentNullException(nameof(environmentSettings));
            if (controlSettings == null) throw new System.ArgumentNullException(nameof(controlSettings));
            if (testCaseReporter == null) throw new System.ArgumentNullException(nameof(testCaseReporter));

            WebDriver = webDriver;
            EnvironmentSettings = environmentSettings;
            ControlSettings = controlSettings;
            TestCaseReporter = testCaseReporter;
        }
    }
}
