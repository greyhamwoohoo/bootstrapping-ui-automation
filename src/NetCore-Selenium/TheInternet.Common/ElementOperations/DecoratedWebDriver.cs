using OpenQA.Selenium;
using System.Collections.ObjectModel;
using TheInternet.Common.ElementOperations.Contracts;

namespace TheInternet.Common.ElementOperations
{
    /// <summary>
    /// Wraps the IWebDriver interface and enriches it. 
    /// </summary>
    public class DecoratedWebDriver : IDecoratedWebDriver
    {
        private readonly IWebDriver _original;

        public DecoratedWebDriver(IWebDriver original)
        {
            if (null == original) throw new System.ArgumentNullException(nameof(original));

            _original = original;
        }
        public void DoSomething()
        {
        }

        #region Wrap _original WebDriver properties and methods

        public string Url { get => _original.Url; set => _original.Url = value; }

        public string Title => _original.Title;

        public string PageSource => _original.PageSource;

        public string CurrentWindowHandle => _original.CurrentWindowHandle;

        public ReadOnlyCollection<string> WindowHandles => _original.WindowHandles;

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
