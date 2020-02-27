using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TheInternet.Common.Infrastructure;

namespace TheInternet.SystemTests.Raw
{
    [TestClass]
    public class OneTimeTestInitialize
    {
        const string PREFIX = "THEINTERNET";

        static IServiceProvider _serviceProvider = default;

        [AssemblyInitialize]
        public static void Initialize(TestContext textContext)
        {
            ContainerSingleton.Initialize(PREFIX);

            _serviceProvider = ContainerSingleton.Instance;
        }

        [AssemblyCleanup]
        public static void Teardown()
        {

        }
    }
}
