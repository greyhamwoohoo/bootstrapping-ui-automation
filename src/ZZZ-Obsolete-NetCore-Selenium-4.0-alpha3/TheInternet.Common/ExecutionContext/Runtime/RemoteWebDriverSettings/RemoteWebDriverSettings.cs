namespace TheInternet.Common.ExecutionContext.Runtime.RemoteWebDriverSettings
{
    public class RemoteWebDriverSettings
    {
        public bool UseRemoteDriver { get; set;  }
        public string RemoteUri { get; set; }

        public RemoteWebDriverSettings()
        {
        }

        internal void Cleanse()
        {
            if (RemoteUri == null) RemoteUri = "http://localhost/no-remoteuri-was-specified-in-the-remote-driver-settings-json-file";
        }
    }
}
