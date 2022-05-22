using Yasf.Common.ExecutionContext.Contracts;

namespace Yasf.Common.ExecutionContext.Runtime.EnvironmentSettings
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
