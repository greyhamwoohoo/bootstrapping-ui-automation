using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Linq;
using static TheInternet.Common.ElementOperations.ElementState;

namespace TheInternet.SystemTests.Raw.Tests
{
    [TestClass]
    public class FileUploadTests : TheInternetTestBase
    {
        protected override string BaseUrl => base.BaseUrl + "/upload";

        [TestMethod]
        public void UploadFile()
        {
            var path = System.IO.Path.Combine(TestContext.TestDeploymentDir, "Content", "SampleFileToUpload.txt");
            var fileuploadElement = Browser.FindElements(By.XPath("//input[@id='file-upload']")).Single();
            fileuploadElement.SendKeys(path);

            var uploadButton = Browser.FindElements(By.XPath("//input[@id='file-submit']")).Single();
            uploadButton.Click();

            BrowserSession.Waiter.AssertThatEventually(By.XPath("//div[@id='uploaded-files']"), Exists);

            BrowserSession.Waiter.AssertThatEventually(browser =>
            {
                browser.PageSource.Should().Contain("SampleFileToUpload.txt", because: "that was the filename uploaded");
            });
        }
    }
}
