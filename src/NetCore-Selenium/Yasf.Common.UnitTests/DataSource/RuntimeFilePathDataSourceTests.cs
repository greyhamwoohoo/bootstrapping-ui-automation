using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Reflection;
using Yasf.Common.DataSource;

namespace Yasf.Common.UnitTests.DataSource
{
    [TestClass]
    public class RuntimeFilePathDataSourceTests
    {
        [TestClass]
        public class WhenAttributed
        {
            [TestMethod]
            [RuntimeFilePathDataSource("DataSource/RuntimeFilePathTestData/OneMatchingFile", "*.json")]
            public void WillFindOneMatchingFile(string filePath)
            {
                var filename = System.IO.Path.GetFileName(filePath);

                filename.Should().Be("thisOneMatches.json");
            }

            [TestMethod]
            [RuntimeFilePathDataSource("DataSource/RuntimeFilePathTestData/TwoMatchingFiles", "*.json")]
            public void WillFindTwoMatchingFiles(string filePath)
            {
                var filename = System.IO.Path.GetFileName(filePath);

                filename.Should().BeOneOf("thisMatches.json", "thisMatchesToo.json");
            }
        }

        [TestClass]
        public class WhenConstructedDirectly
        {
            // We do not use the MethodInfo for anything in the datasource - so we can just fill this with anything. 
            private MethodInfo throwAwayMethodInfo = default;
            private RuntimeFilePathDataSource dataSource = default;

            [TestInitialize]
            public void Setup()
            {
                throwAwayMethodInfo = typeof(WhenConstructedDirectly).GetMethods().First();
            }

            [TestMethod]
            public void WillFindNoFilesIfADirectoryDoesNotExist()
            {
                // Arrange
                dataSource = new RuntimeFilePathDataSource("ThisPathDoesNotExist", "*.json");

                // Act
                var filePaths = dataSource.GetData(throwAwayMethodInfo).ToList();

                // Assert
                filePaths.Count().Should().Be(0, because: "that path does not exist. ");
            }

            [TestMethod]
            public void WillFindNoMatchesIfTheFolderExistsButNoFilesMatchThePattern()
            {
                // Arrange
                dataSource = new RuntimeFilePathDataSource("DataSource", "*.notjson");

                // Act
                var filePaths = dataSource.GetData(throwAwayMethodInfo).ToList();

                // Assert
                filePaths.Count().Should().Be(0, because: "the path exists but there were no matching patterns. ");
            }

            [TestMethod]
            public void WillFindOneFileThatMatchesThePattern()
            {
                // Arrange
                dataSource = new RuntimeFilePathDataSource("DataSource/RuntimeFilePathTestData/OneMatchingFile", "*.json");

                // Act
                var filePaths = dataSource.GetData(throwAwayMethodInfo).ToList();

                // Assert
                filePaths.Count().Should().Be(1, because: "there is exactly one matching file under that path. ");

                var path = $"{filePaths[0][0]}";
                var filename = System.IO.Path.GetFileName(path);
                filename.Should().Be("thisOneMatches.json", because: "there is only one match");
            }

            [TestMethod]
            public void WillFindTwoFilesThatMatchesThePattern()
            {
                // Arrange
                dataSource = new RuntimeFilePathDataSource("DataSource/RuntimeFilePathTestData/TwoMatchingFiles", "*.json");

                // Act
                var filePaths = dataSource.GetData(throwAwayMethodInfo).ToList();

                // Assert
                filePaths.Count().Should().Be(2, because: "there are two matching files. ");

                var path1 = $"{filePaths[0][0]}";
                var path2 = $"{filePaths[1][0]}";
                var filename1 = System.IO.Path.GetFileName(path1);
                var filename2 = System.IO.Path.GetFileName(path2);

                filename1.Should().BeOneOf("thisMatches.json", "thisMatchesToo.json");
                filename2.Should().BeOneOf("thisMatches.json", "thisMatchesToo.json");
            }

            [TestMethod]
            public void WillMatchFilesRecursively()
            {
                // Arrange
                dataSource = new RuntimeFilePathDataSource("DataSource/RuntimeFilePathTestData", "*.json");

                // Act
                var filePaths = dataSource.GetData(throwAwayMethodInfo).ToList();

                // Assert
                filePaths.Count().Should().Be(3, because: "there are three matching files in total for the runtime file test. ");
            }
        }
    }
}
