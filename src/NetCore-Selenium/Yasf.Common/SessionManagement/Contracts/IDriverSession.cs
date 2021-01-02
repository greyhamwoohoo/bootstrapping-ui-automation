using OpenQA.Selenium;
using Yasf.Common.ElementOperations.Contracts;
using Yasf.Common.ExecutionContext.Runtime.ControlSettings;
using Yasf.Common.ExecutionContext.Runtime.EnvironmentSettings;
using Yasf.Common.Reporting.Contracts;

namespace Yasf.Common.SessionManagement.Contracts
{
    public interface IDriverSession
    {
        EnvironmentSettings EnvironmentSettings { get; }
        IControlSettings ControlSettings { get; }
        IDecoratedWebDriver WebDriver { get; }
        ITestCaseReporter TestCaseReporter { get; }
    }
}
