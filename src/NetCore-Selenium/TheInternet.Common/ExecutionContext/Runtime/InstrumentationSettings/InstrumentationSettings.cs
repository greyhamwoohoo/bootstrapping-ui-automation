using TheInternet.Common.ExecutionContext.Contracts;

namespace TheInternet.Common.ExecutionContext.Runtime.InstrumentationSettings
{
    /// <summary>
    /// Placeholder for controlling things such as: reporting and logging storage. 
    /// </summary>
    public class InstrumentationSettings : IInstrumentationSettings, ICleanse
    {
        public InstrumentationSettings()
        {
        }

        public bool LogFilePerTest { get; set; }
        public string RootReportingFolder { get; set; }

        public ReportingSettings ReportingSettings { get; set; }

        IReportingSettings IInstrumentationSettings.ReportingSettings => ReportingSettings;

        public void Cleanse()
        {
            ReportingSettings ??= new ReportingSettings();

            (ReportingSettings as ICleanse)?.Cleanse();
        }
    }

    public class ReportingSettings : ICleanse, IReportingSettings
    {
        public bool Enabled { get; set; }
        public bool AlwaysCaptureScreenshots { get; set; }
        public void Cleanse()
        {
        }
    }
}
