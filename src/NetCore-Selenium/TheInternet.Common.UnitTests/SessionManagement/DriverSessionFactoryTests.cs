using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Idioms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheInternet.Common.SessionManagement;

namespace TheInternet.Common.UnitTests.SessionManagement
{
    [TestClass]
    public class DriverSessionFactoryTests
    {
        [TestMethod]
        public void GuardTests()
        {
            // Arrange
            var fixture = new Fixture();

            fixture.Customize(new AutoNSubstituteCustomization());

            var assertThatAllMembersHaveGuards = new GuardClauseAssertion(fixture);

            // Act, Assert
            assertThatAllMembersHaveGuards.Verify(typeof(DriverSessionFactory).GetMembers());
        }
    }
}
