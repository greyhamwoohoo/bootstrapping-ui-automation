using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Idioms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TheInternet.Common.Reporting;

namespace TheInternet.Common.UnitTests.SessionManagement
{
    [TestClass]
    public class TestCaseReporterTests
    {
        [TestMethod]
        public void All_Methods_Have_Guards_Except_Logging_Methods()
        {
            // Arrange
            var fixture = new Fixture();

            fixture.Customize(new AutoNSubstituteCustomization());

            var assertThatAllMembersHaveGuards = new GuardClauseAssertion(fixture);

            // Act, Assert
            assertThatAllMembersHaveGuards.Verify(typeof(TestCaseReporter).GetConstructors());
            assertThatAllMembersHaveGuards.Verify(typeof(TestCaseReporter).GetMembers().Single(x => x.Name == "Initialize"));
        }
    }
}
