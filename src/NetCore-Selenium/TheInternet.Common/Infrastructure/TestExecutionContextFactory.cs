using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace TheInternet.Common.Infrastructure
{
    public static class TestExecutionContextFactory
    {
        public static TestExecutionContext Create(string testExecutionContextFullPath)
        {
            var configuration = BuildTestExecutionContext(testExecutionContextFullPath);
            var textExecutionContext = configuration.GetSection("TestExecutionContext").Get<TestExecutionContext>();
            return textExecutionContext;
        }

        private static IConfigurationRoot BuildTestExecutionContext(string testExecutionContextFilename)
        {
            var testExecutionContextRelativePath = Path.Combine("TestExecutionContexts", $"{testExecutionContextFilename}");
            var testExecutionContextFullPath = Path.Combine(Directory.GetCurrentDirectory(), testExecutionContextRelativePath);
            if (!File.Exists(testExecutionContextFullPath))
            {
                throw new FileNotFoundException($"The TestExecutionContext file does not exist at '{testExecutionContextFullPath}");
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Path.Combine("TestExecutionContexts", "tec.json"), optional: false)
                .AddJsonFile(testExecutionContextRelativePath, optional: false)
                .Build();

            return configuration;
        }
    }
}
