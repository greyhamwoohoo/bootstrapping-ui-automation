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
        public void Cleanse()
        {
        }
    }
}
