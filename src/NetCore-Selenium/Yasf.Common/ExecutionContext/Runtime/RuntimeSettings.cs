using Microsoft.Extensions.Configuration;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Yasf.Common.ExecutionContext.Runtime
{
    public class RuntimeSettings
    {
        private readonly ILogger _logger;

        public RuntimeSettings(ILogger logger)
        {
            if (null == logger) throw new System.ArgumentNullException(nameof(logger));

            _logger = logger;
        }

        /// <summary>
        /// Will return a list of all settings files. 
        /// </summary>
        /// <remarks>
        /// An environment variable will be queried by convention: PREFIXCATEGORY_FILES</remarks>
        /// <param name="prefix">The prefix used for this service. There must always be a prefix. Typically: this is the name of the service. </param>
        /// <param name="category">The category of settings. Use one of the folders under the Runtime folder. </param>
        /// <param name="rootFolder">Root folder. Typically: the test execution folder. </param>
        /// <param name="defaultFilename">Default settings filename. </param>
        /// <returns></returns>
        public IEnumerable<string> CalculatePathsOfSettingsFiles(string prefix, string rootFolder, string category, string defaultFilename)
        {
            if (rootFolder == null) throw new System.ArgumentNullException(nameof(rootFolder));
            if (prefix == null) throw new System.ArgumentNullException(nameof(prefix));
            if (category == null) throw new System.ArgumentNullException(nameof(category));
            if (defaultFilename == null) throw new System.ArgumentNullException(nameof(defaultFilename));

            _logger.Information($"GetSettingsFile. Prefix={prefix}, rootFolder={rootFolder}, category={category}, defaultFilename={defaultFilename}");

            string environmentVariableName = $"%{prefix}{category.ToUpper()}_FILES%";
            _logger.Information($"Environment variable that will contain (optional, non-default) variables is called: {environmentVariableName}");

            var candidateSettingFiles = System.Environment.ExpandEnvironmentVariables(environmentVariableName);
            if (candidateSettingFiles == $"%{prefix}{category.ToUpper()}_FILES%")
            {
                _logger.Information($"The environment variable '{environmentVariableName}' has not been set. Will use the default filename instead. ");
                candidateSettingFiles = Path.Combine(rootFolder, category, defaultFilename);
            }
            else
            {
                _logger.Information($"The environment variable '{environmentVariableName}' is already set to {candidateSettingFiles}");
            }

            var settingsPaths = candidateSettingFiles.Split(";").ToList().Select(path =>
            {
                if (!Path.IsPathRooted(path))
                {
                    return Path.Combine(rootFolder, category, path);
                }
                else
                {
                    return path;
                }
            });

            _logger.Information($"Will use the following paths to retrieve settings: ");
            settingsPaths.ToList().ForEach(path =>
            {
                _logger.Information($"   {path}");
            });

            return settingsPaths;
        }

        /// <summary>
        /// Builds a Configuration object using the .Net Core 
        /// </summary>
        /// <param name="prefix">Environment variable prefix to use for overrides. </param>
        /// <param name="paths">Fully qualified paths for the configuration. Must exist. </param>
        /// <returns></returns>
        public IConfigurationRoot BuildConfiguration(string prefix, IEnumerable<string> paths)
        {
            if (null == prefix) throw new System.ArgumentNullException(nameof(prefix));
            if (null == paths) throw new System.ArgumentNullException(nameof(paths));

            var configuration = new ConfigurationBuilder();

            paths.ToList().ForEach(path =>
            {
                configuration.AddJsonFile(path, optional: false, reloadOnChange: false);
            });

            configuration.AddEnvironmentVariables(prefix);

            var result = configuration.Build();
            return result;
        }
    }
}
