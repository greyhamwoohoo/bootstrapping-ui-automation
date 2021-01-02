using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace TheInternet.Common.DataSource
{
    /// <summary>
    /// DataSource that will dynamically discover the configuration files relative to the .Common assembly.
    /// </summary>
    public class RuntimeFilePathDataSource : Attribute, ITestDataSource
    {
        private readonly string Path;
        private readonly string RelativePath;
        private readonly string Pattern;

        public RuntimeFilePathDataSource(string relativePath, string pattern)
        {
            RelativePath = relativePath ?? throw new System.ArgumentNullException(nameof(relativePath));
            Pattern = pattern ?? throw new System.ArgumentNullException(nameof(pattern));

            Path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(typeof(RuntimeFilePathDataSource).Assembly.Location), relativePath);
        }

        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            if (!System.IO.Directory.Exists(Path)) yield break;

            var runtimeFilePaths = System.IO.Directory.GetFiles(Path, Pattern, System.IO.SearchOption.AllDirectories);

            if (runtimeFilePaths.Length == 0) yield break;

            foreach (var runtimeFilePath in runtimeFilePaths)
            {
                yield return new object[] { runtimeFilePath };
            };
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            var runtimeFilePath = $"{data[0]}";

            var name = System.IO.Path.GetRelativePath(Path, runtimeFilePath);

            return name;
        }
    }
}
