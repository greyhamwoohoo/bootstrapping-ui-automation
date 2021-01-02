using Yasf.Common.ExecutionContext.Contracts;

namespace Yasf.Common.ExecutionContext.Runtime.ControlSettings
{
    public class ControlSettings : IControlSettings, ICleanse
    {
        public ControlSettings()
        {
            PollingTimeInMilliseconds = 250;
            WaitUntilTimeoutInSeconds = 30;
            AppiumDriverCreationRetries = 1;
            AttachToExistingSessionIfItExists = false;
        }

        public int PollingTimeInMilliseconds { get; set; }
        public int WaitUntilTimeoutInSeconds { get; set; }
        public bool AttachToExistingSessionIfItExists { get; set; }
        public int AppiumDriverCreationRetries { get; set; }
        public void Cleanse()
        {
        }
    }
}
