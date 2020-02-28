namespace TheInternet.Common.ExecutionContext.Runtime.RemoteWebDriverSettings
{
    public class EnvironmentSettings
    {
        public string BaseUrl { get; set; }

        internal void Cleanse()
        {
            if (BaseUrl == null) BaseUrl = "http://localhost/you-need-to-set-environment-base-url";
        }
    }
}
