using TheInternet.Common.ExecutionContext.Runtime.InstrumentationSettings;
using TheInternet.Common.Reporting.Contracts;

namespace TheInternet.Common.Reporting
{
    public class TestRunReporterContext : ITestRunReporterContext
    {
        public string RootOutputFolder { get; internal set; }

        public string TestRunIdentity { get; internal set; }

        public IInstrumentationSettings InstrumentationSettings { get; internal set; }
    }
}
