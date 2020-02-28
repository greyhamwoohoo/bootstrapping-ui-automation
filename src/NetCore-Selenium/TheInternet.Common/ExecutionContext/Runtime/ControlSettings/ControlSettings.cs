using System;
using System.Collections.Generic;
using System.Text;

namespace TheInternet.Common.ExecutionContext.Runtime.ControlSettings
{
    public class ControlSettings : IControlSettings
    {
        public ControlSettings()
        {
            PollingTimeInMilliseconds = 250;
            WaitUntilTimeoutInSeconds = 30;
        }

        public int PollingTimeInMilliseconds { get; set; }
        public int WaitUntilTimeoutInSeconds { get; set; }

        public void Cleanse()
        {
        }
    }
}
