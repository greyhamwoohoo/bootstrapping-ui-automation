using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System.Collections.Generic;
using System.IO;
using TheInternet.Common.Drivers;

namespace TheInternet.Common.WebDrivers
{
    /// <summary>
    /// Allows us to attach to a running Internet Explorer Driver instance / Selenium Session
    /// </summary>
    public class AttachableInternetExplorerDriver : InternetExplorerDriver
    {
        public AttachableInternetExplorerDriver(InternetExplorerOptions options) : base(options)
        {
        }

        protected override Response Execute(string driverCommandToExecute, Dictionary<string, object> parameters)
        {
            var decoratedRemoteWebDriver = new DriverDecorator(this, "InternetExplorer", Directory.GetCurrentDirectory());
            decoratedRemoteWebDriver.AssertSeleniumVersionIsCompatible();

            return decoratedRemoteWebDriver.Execute(driverCommandToExecute, parameters, Executor);
        }

        private Response Executor(string driverCommandToExecute, Dictionary<string, object> parameters)
        {
            // We need access to base.Execute as there is no clear way to 'hook in' to the web driver architecture the way I need to
            return base.Execute(driverCommandToExecute, parameters);
        }
    }
}
