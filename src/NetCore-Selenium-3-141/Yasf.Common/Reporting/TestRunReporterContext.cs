using Yasf.Common.ExecutionContext.Runtime.InstrumentationSettings;
using Yasf.Common.Reporting.Contracts;

namespace Yasf.Common.Reporting
{
    public class TestRunReporterContext : ITestRunReporterContext
    {
        public string RootReportingFolder { get; internal set; }

        public string TestRunIdentity { get; internal set; }

        public IInstrumentationSettings InstrumentationSettings { get; internal set; }
    }
}
