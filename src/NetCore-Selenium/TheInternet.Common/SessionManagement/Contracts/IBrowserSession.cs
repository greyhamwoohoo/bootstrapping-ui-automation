using OpenQA.Selenium;

namespace TheInternet.Common.SessionManagement.Contracts
{
    public interface IBrowserSession
    {
        IWebDriver WebDriver { get; }
    }
}
