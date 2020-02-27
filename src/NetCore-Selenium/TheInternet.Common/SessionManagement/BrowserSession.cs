using OpenQA.Selenium;
using TheInternet.Common.SessionManagement.Contracts;

namespace TheInternet.Common.SessionManagement
{
    public class BrowserSession : IBrowserSession
    {
        public IWebDriver WebDriver { get; }

        public BrowserSession(IWebDriver webDriver) 
        {
            if (webDriver == null) throw new System.ArgumentNullException(nameof(webDriver));

            WebDriver = webDriver;
        }
    }
}
