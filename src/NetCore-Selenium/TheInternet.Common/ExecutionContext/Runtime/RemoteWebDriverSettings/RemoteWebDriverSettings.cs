using TheInternet.Common.ExecutionContext.Contracts;

namespace TheInternet.Common.ExecutionContext.Runtime.RemoteWebDriverSettings
{
    public class RemoteWebDriverSettings : ICleanse
    {
        public int HttpCommandExecutorTimeoutInSeconds { get; set; }
        public bool UseRemoteDriver { get; set;  }
        public string RemoteUri { get; set; }

        public RemoteWebDriverSettings()
        {
        }
        public void Cleanse()
        {
            RemoteUri ??= "http://localhost/no-remoteuri-was-specified-in-the-remote-driver-settings-json-file";
        }
    }
}
