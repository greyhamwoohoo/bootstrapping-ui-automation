using OpenQA.Selenium.Remote;
using System;

namespace TheInternet.Common.Drivers
{
    /// <summary>
    /// Decorates the Driver by providing an accessible facade over its private / protected methods to enable session re-attach
    /// </summary>
    public class DriverDecorator
    {
        public const string IMPLEMENTATION_NOTE = "This implementation is for Selenium 3.141.59 and is using protected / private methods to enable session reuse. The implementation might have changed for the Selenium you are using; we will need a new specific adapter for the (Driver,SeleniumVersion).";

        private readonly RemoteWebDriver _remoteWebDriver;

        public DriverDecorator(RemoteWebDriver remoteWebDriver)
        {
            if (null == remoteWebDriver) throw new System.ArgumentNullException(nameof(remoteWebDriver));

            _remoteWebDriver = remoteWebDriver;
        }

        public void AssertSeleniumVersionIsCompatible()
        {
            var seleniumVersion = typeof(RemoteWebDriver).Assembly.GetName().Version.ToString();
            if (seleniumVersion != "3.0.0.0" && seleniumVersion != "3.141.0.0")
            {
                // Unfortunately, it would appear that 3.141.59 is not surfaced anywhere, so we are limited to major/minor
                throw new NotSupportedException($"{IMPLEMENTATION_NOTE}");
            }
        }

        public string GetRemoteServerUri()
        {
            // return this.CommandExecutor.HttpExecutor.remoteServerUri.ToString();
            var commandExecutor = GetCommandExecutor(_remoteWebDriver);
            
            var httpExecutorProperty = commandExecutor.GetType().GetProperty("HttpExecutor");
            if (null == httpExecutorProperty) throw new System.InvalidOperationException($"remoteWebDriver.CommandExecutor.HttpExecutor property should exist. {IMPLEMENTATION_NOTE}");
            
            var executor = httpExecutorProperty.GetValue(commandExecutor);
            if (null == executor) throw new System.InvalidOperationException($"remoteWebDriver.CommandExecutor.HttpExecutor property should not be null. {IMPLEMENTATION_NOTE}");

            var remoteServerUriField = executor.GetType().GetField("remoteServerUri", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (null == remoteServerUriField) throw new System.InvalidOperationException($"remoteWebDriver.CommandExecutor.HttpExecutor.remoteServerUriField property should not be null. {IMPLEMENTATION_NOTE}");

            var remoteServerUri = remoteServerUriField.GetValue(executor);
            if (null == remoteServerUri) throw new System.InvalidOperationException($"remoteWebDriver.CommandExecutor.HttpExecutor.remoteServerUri should not be null. {IMPLEMENTATION_NOTE}");

            return remoteServerUri.ToString();
        }

        public void SetRemoteServerUri(string value)
        {
            // this.CommandExecutor.HttpExecutor.remoteServerUri = new Uri(value)
            var commandExecutor = GetCommandExecutor(_remoteWebDriver);

            var httpExecutorProperty = commandExecutor.GetType().GetProperty("HttpExecutor");
            if (null == httpExecutorProperty) throw new System.InvalidOperationException($"remoteWebDriver.CommandExecutor.HttpExecutor property should exist. {IMPLEMENTATION_NOTE}");

            var executor = httpExecutorProperty.GetValue(commandExecutor);
            if (null == executor) throw new System.InvalidOperationException($"remoteWebDriver.CommandExecutor.HttpExecutor property should not be null. {IMPLEMENTATION_NOTE}");

            var remoteServerUriField = executor.GetType().GetField("remoteServerUri", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (null == remoteServerUriField) throw new System.InvalidOperationException($"remoteWebDriver.CommandExecutor.HttpExecutor.remoteServerUriField property should not be null. {IMPLEMENTATION_NOTE}");

            remoteServerUriField.SetValue(executor, new Uri(value));
        }

        public CommandInfoRepository GetCommandInfoRepository()
        {
            // return this.CommandExecutor.HttpExecutor.commandInfoRepository
            var commandExecutor = GetCommandExecutor(_remoteWebDriver);

            var httpExecutorProperty = commandExecutor.GetType().GetProperty("HttpExecutor");
            if (null == httpExecutorProperty) throw new System.InvalidOperationException($"remoteWebDriver.CommandExecutor.HttpExecutor property should exist. {IMPLEMENTATION_NOTE}");

            var executor = httpExecutorProperty.GetValue(commandExecutor);
            if (null == executor) throw new System.InvalidOperationException($"remoteWebDriver.CommandExecutor.HttpExecutor property should not be null. {IMPLEMENTATION_NOTE}");
            
            var commandInfoRepositoryField = executor.GetType().GetField("commandInfoRepository", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (null == commandInfoRepositoryField) throw new System.InvalidOperationException($"remoteWebDriver.CommandExecutor.HttpExecutor.commandInfoRepository property should exist. {IMPLEMENTATION_NOTE}");

            var commandInfoRepository = commandInfoRepositoryField.GetValue(executor);
            if (null == commandInfoRepository) throw new System.InvalidOperationException($"remoteWebDriver.CommandExecutor.HttpExecutor.commandInfoRepository property should not be null. {IMPLEMENTATION_NOTE}");

            var result = commandInfoRepository as CommandInfoRepository;
            if (null == result) throw new System.InvalidOperationException($"remoteWebDriver.CommandExecutor.HttpExecutor.commandInfoRepository property should be of type CommandInfoRepository. {IMPLEMENTATION_NOTE}");
            
            return result;
        }

        public void SetCommandInfoRepository(CommandInfoRepository repository)
        {
            // this.CommandExecutor.HttpExecutor.commandInfoRepository = repository
            var commandExecutor = GetCommandExecutor(_remoteWebDriver);

            var httpExecutorProperty = commandExecutor.GetType().GetProperty("HttpExecutor");
            if (null == httpExecutorProperty) throw new System.InvalidOperationException($"remoteWebDriver.CommandExecutor.HttpExecutor property should exist. {IMPLEMENTATION_NOTE}");

            var executor = httpExecutorProperty.GetValue(commandExecutor);
            if (null == executor) throw new System.InvalidOperationException($"remoteWebDriver.CommandExecutor.HttpExecutor property should not be null. {IMPLEMENTATION_NOTE}");
           
            var commandInfoRepositoryField = executor.GetType().GetField("commandInfoRepository", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (null == commandInfoRepositoryField) throw new System.InvalidOperationException($"remoteWebDriver.CommandExecutor.HttpExecutor.commandInfoRepository property should exist. {IMPLEMENTATION_NOTE}");

            commandInfoRepositoryField.SetValue(executor, repository);
        }

        private ICommandExecutor GetCommandExecutor(RemoteWebDriver remoteWebDriver)
        {
            var commandExecutorProperty = remoteWebDriver.GetType().GetProperty("CommandExecutor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (null == commandExecutorProperty) throw new System.InvalidOperationException($"remoteWebDriver.CommandExecutor property should exist. {IMPLEMENTATION_NOTE}");

            var commandExecutor = commandExecutorProperty.GetValue(remoteWebDriver) as ICommandExecutor;
            if (null == commandExecutor) throw new System.InvalidOperationException($"remoteWebDriver.CommandExecutor should be of type ICommandExecutor. {IMPLEMENTATION_NOTE}");

            return commandExecutor;
        }
    }
}
