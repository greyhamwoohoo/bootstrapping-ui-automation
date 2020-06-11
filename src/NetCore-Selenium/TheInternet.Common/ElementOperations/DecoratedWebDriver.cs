using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System.Collections.ObjectModel;
using TheInternet.Common.ElementOperations.Contracts;
using TheInternet.Common.ExecutionContext.Runtime.ControlSettings;

namespace TheInternet.Common.ElementOperations
{
    /// <summary>
    /// Wraps the IWebDriver interface and enriches it. 
    /// </summary>
    public class DecoratedWebDriver : IDecoratedWebDriver, IWrapsDriver
    {
        private readonly IWebDriver _original;
        private readonly IControlSettings _controlSettings;

        public DecoratedWebDriver(IWebDriver original, IControlSettings controlSettings)
        {
            _original = original ?? throw new System.ArgumentNullException(nameof(original));
            _controlSettings = controlSettings ?? throw new System.ArgumentNullException(nameof(controlSettings));

            Assert = new WebDriverAssertions(this, _controlSettings);
        }
        
        public IWebDriverAssertions Assert { get; }

        #region Wrap _original WebDriver properties and methods

        public string Url { get => _original.Url; set => _original.Url = value; }

        public string Title => _original.Title;

        public string PageSource => _original.PageSource;

        public string CurrentWindowHandle => _original.CurrentWindowHandle;

        public ReadOnlyCollection<string> WindowHandles => _original.WindowHandles;

        public IWebDriver WrappedDriver => _original;

        public void Close()
        {
            _original.Close();
        }

        public void Dispose()
        {
            _original.Dispose();
        }

        public IWebElement FindElement(By by)
        {
            return _original.FindElement(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return _original.FindElements(by);
        }

        public IOptions Manage()
        {
            return _original.Manage();
        }

        public INavigation Navigate()
        {
            return _original.Navigate();
        }

        public void Quit()
        {
            _original.Quit();
        }

        public ITargetLocator SwitchTo()
        {
            return _original.SwitchTo();
        }

        #endregion
    }
}
