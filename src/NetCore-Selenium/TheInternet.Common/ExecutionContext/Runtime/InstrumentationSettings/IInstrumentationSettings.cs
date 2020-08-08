namespace TheInternet.Common.ExecutionContext.Runtime.InstrumentationSettings
{
    public interface IInstrumentationSettings
    {
        bool LogFilePerTest {get; }
        IReportingSettings ReportingSettings { get; }
    }

    public interface IReportingSettings
    {
        bool Enabled { get; }
        bool AlwaysCaptureScreenshots { get; }
    }
}
