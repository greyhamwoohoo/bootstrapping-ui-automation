using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TheInternet.Common.UnitTests
{
    [TestClass]
    public abstract class TestBase
    {
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Return the fully qualified paths of all files matching the pattern under relativePath
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<string> DiscoverFilesAtPath(string relativePath, string pattern)
        {
            var root = System.IO.Path.Combine(TestContext.DeploymentDirectory, relativePath);

            var jsonFiles = System.IO.Directory.GetFiles(root, pattern, System.IO.SearchOption.AllDirectories);

            return jsonFiles;
        }
    }
}
