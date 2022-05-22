using OpenQA.Selenium.Edge;

namespace Yasf.Common.ExecutionContext.Runtime.BrowserSettings
{
    public class EdgeBrowserSettingsAdapter
    {
        public EdgeOptions ToEdgeOptions(EdgeBrowserSettings settings)
        {
            var result = new EdgeOptions();
            if(settings.Options.UseInPrivateBrowsing)
            {
                result.AddArgument("inprivate");
            }
            return result;
        }
    }
}
