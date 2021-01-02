using TheInternet.Common.ExecutionContext.Contracts;

namespace TheInternet.Common.ExecutionContext.Runtime.RemoteWebDriverSettings
{
    public class EnvironmentSettings : ICleanse
    {
        public string BaseUrl { get; set; }

        public void Cleanse()
        {
            if (BaseUrl == null) BaseUrl = "http://localhost/you-need-to-set-environment-base-url";
        }
    }
}
