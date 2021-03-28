using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TheInternet.SystemTests.Raw.Infrastructure
{
    public static class TestSettingsFactory
    {
        public static TestSettings Create(string testExecutionContextFullPath)
        {
            var configuration = BuildTestExecutionContext(testExecutionContextFullPath);
            var testSettings = configuration.GetSection("TestSettings").Get<TestSettings>();
            return testSettings;
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
                .AddJsonFile(Path.Combine("TestExecutionContexts", "testsettings.json"), optional: false)
                .AddJsonFile(testExecutionContextRelativePath, optional: false)
                .Build();

            return configuration;
        }
    }
}
