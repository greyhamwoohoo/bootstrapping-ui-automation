using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Linq;
using TheInternet.Common.DataSource;

namespace TheInternet.Common.UnitTests.Runtime
{
    [TestClass]
    public class RuntimeFileTests
    {
        [TestClass]
        public class WhenRuntimeFilesAreDeployed : TestBase
        {
            [TestMethod]
            public void ThereAreManyFiles()
            {
                DiscoverFilesAtPath("Runtime", "*.json").Count().Should().BeGreaterThan(10, because: "we will be parsing all Json files. ");
            }

            [TestMethod]
            [RuntimeFilePathDataSource(relativePath: "Runtime", pattern: "*.json")]
            public void AllRuntimeFilesCanBeParsed(string runtimeFilePath)
            {
                var content = System.IO.File.ReadAllText(runtimeFilePath);

                _ = JsonConvert.DeserializeObject(content);
            }
        }
    }
}
