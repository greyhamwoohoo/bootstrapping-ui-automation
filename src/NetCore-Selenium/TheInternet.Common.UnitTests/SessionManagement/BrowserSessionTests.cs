using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Support.Events;
using System.Linq;
using TheInternet.Common.SessionManagement;

namespace TheInternet.Common.UnitTests.SessionManagement
{
    [TestClass]
    public class BrowserSessionTests
    {
        [TestMethod]
        public void GuardTests()
        {
            // Arrange
            var fixture = new Fixture();

            fixture.Customize(new AutoNSubstituteCustomization());

            var assertThatAllMembersHaveGuards = new GuardClauseAssertion(fixture);

            // Act, Assert
            assertThatAllMembersHaveGuards.Verify(typeof(BrowserSession).GetMembers());
        }
    }
}
