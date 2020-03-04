using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using System;
using TheInternet.Common.Infrastructure;

namespace TheInternet.SystemTests.Raw
{
    [TestClass]
    public abstract class TestBase
    {
        protected IServiceScope Scope;
        protected ILogger Logger => Scope.ServiceProvider.GetRequiredService<ILogger>();

        [TestInitialize]
        public void SetupTest()
        {
            Scope = ContainerSingleton.Instance.CreateScope();
        }

        [TestCleanup]
        public void TeardownTest()
        {
            Scope?.Dispose();
        }

        protected T Resolve<T>()
        {
            var result = Scope.ServiceProvider.GetService<T>();
            if (result == null) throw new InvalidOperationException($"The type {typeof(T).FullName} is not registered in the Container. Update the ContainerSingleton.cs file. ");

            return result;
        }
    }
}
