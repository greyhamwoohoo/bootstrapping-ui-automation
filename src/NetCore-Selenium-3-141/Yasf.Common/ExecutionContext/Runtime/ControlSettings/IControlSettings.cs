namespace Yasf.Common.ExecutionContext.Runtime.ControlSettings
{
    /// <summary>
    /// We use an interface for the 'Common' control settings. 
    /// </summary>
    public interface IControlSettings
    {
        int WaitUntilTimeoutInSeconds { get; }
        int PollingTimeInMilliseconds { get; }
        bool AttachToExistingSessionIfItExists { get; }
        int AppiumDriverCreationRetries { get; }
    }
}
