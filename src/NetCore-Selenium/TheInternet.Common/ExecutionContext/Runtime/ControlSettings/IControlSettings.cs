using System;
using System.Collections.Generic;
using System.Text;

namespace TheInternet.Common.ExecutionContext.Runtime.ControlSettings
{
    /// <summary>
    /// We use an interface for the 'Common' control settings. 
    /// </summary>
    public interface IControlSettings
    {
        public int WaitUntilTimeoutInSeconds { get; }
        public int PollingTimeInMilliseconds { get; }
    }
}
