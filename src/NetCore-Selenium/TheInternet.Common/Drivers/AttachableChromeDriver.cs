using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.IO;
using TheInternet.Common.SessionManagement;

namespace TheInternet.Common.WebDrivers
{
    /// <summary>
    /// Allows us to attach to a running Chrome Driver instance / Selenium Session
    /// </summary>
    /// <remarks>
    /// The AttachableDrivers have almost identical code - will refactor later.
    /// </remarks>
    public class AttachableChromeDriver : ChromeDriver
    {
        const string BROWSER_NAME = "CHROME";

        public AttachableChromeDriver(ChromeOptions options) : base(options)
        {
        }

        protected override Response Execute(string driverCommandToExecute, Dictionary<string, object> parameters)
        {
            var seleniumVersion = typeof(RemoteWebDriver).Assembly.GetName().Version.ToString();
            if(seleniumVersion != "3.0.0.0" && seleniumVersion != "3.141.0.0")
            {
                // Unfortunately, it would appear that 3.141.59 is not surfaced anywhere, so we are limited to major/minor
                throw new NotSupportedException($"The implementation below is very specific to the above Selenium Versions (3.0.0.0 if referencing the Webdriver code locally; 3.141.0.0 if referencing the NuGET package. ");
            }

            //
            // NOTE: *ALL* of the following code requires Selenium 3.141.59 to work (it might work on earlier versions - but definitely not 4)
            //
            if (driverCommandToExecute == "newSession")
            {
                var attachableSeleniumSessionStorage = new AttachableSeleniumSessionStorage(Directory.GetCurrentDirectory());
                var existingSession = attachableSeleniumSessionStorage.ReadSessionState(BROWSER_NAME);
                var sessionProbe = new AttachableSeleniumSessionProbe();
                
                if(!existingSession.IsValid || existingSession.BrowserName != BROWSER_NAME || !sessionProbe.IsRunning(existingSession))
                {
                    attachableSeleniumSessionStorage.RemoveSessionState();
                    existingSession = attachableSeleniumSessionStorage.ReadSessionState(BROWSER_NAME);
                }

                if(!existingSession.IsValid)
                {
                    // There is currently no persisted session we can use. 
                    var newSession = base.Execute(driverCommandToExecute, parameters);
                    if (newSession.Status != WebDriverResult.Success) return newSession;

                    var attachableSeleniumSession = new AttachableSeleniumSession()
                    {
                        BrowserName = BROWSER_NAME,
                        Response = newSession,
                        OfficialResponse = newSession.ToJson(),
                        RemoteServerUri = GetRemoteServerUri(),
                        CommandRepositoryTypeName = GetCommandInfoRepository().GetType().FullName
                    };

                    attachableSeleniumSessionStorage.WriteSessionState(attachableSeleniumSession);

                    return newSession;
                }

                // The HttpCommandExecutor has some specific logic if handling the NewSession command - it will determine
                // the specification and update the internal CommandInfoRepository. This is what gives the 'specification level'
                // of the connection. Therefore, as we skipped that call and constructed the session ourselves, we need to inject
                // the CommandInfoRepository here. 
                if(existingSession.CommandRepositoryTypeName == typeof(W3CWireProtocolCommandInfoRepository).FullName)
                {
                    SetCommandInfoRepository(new W3CWireProtocolCommandInfoRepository());
                }
                else if(existingSession.CommandRepositoryTypeName == typeof(WebDriverWireProtocolCommandInfoRepository).FullName)
                {
                    SetCommandInfoRepository(new WebDriverWireProtocolCommandInfoRepository());
                }
                else
                {
                    throw new System.InvalidOperationException($"At the time of writing this there were two implementations of CommandInfoRepository. Add a switch statement and new up a type of {existingSession.CommandRepositoryTypeName}");
                }

                SetRemoteServerUri(existingSession.RemoteServerUri);
                return existingSession.Response;
            }

            var result = base.Execute(driverCommandToExecute, parameters);
            return result;
        }

        private string GetRemoteServerUri()
        {
            var httpExecutorProperty = this.CommandExecutor.GetType().GetProperty("HttpExecutor");
            var executor = httpExecutorProperty.GetValue(this.CommandExecutor);
            var remoteServerUriField = executor.GetType().GetField("remoteServerUri", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var remoteServerUri = remoteServerUriField.GetValue(executor);
            return remoteServerUri.ToString();
        }

        private void SetRemoteServerUri(string value)
        {
            var httpExecutorProperty = this.CommandExecutor.GetType().GetProperty("HttpExecutor");
            var executor = httpExecutorProperty.GetValue(this.CommandExecutor);
            var remoteServerUriField = executor.GetType().GetField("remoteServerUri", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            remoteServerUriField.SetValue(executor, new Uri(value));
        }

        private CommandInfoRepository GetCommandInfoRepository()
        {
            var httpExecutorProperty = this.CommandExecutor.GetType().GetProperty("HttpExecutor");
            var executor = httpExecutorProperty.GetValue(this.CommandExecutor);
            var commandInfoRepositoryField = executor.GetType().GetField("commandInfoRepository", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var commandInfoRepository = commandInfoRepositoryField.GetValue(executor);
            return commandInfoRepository as CommandInfoRepository;
        }

        private void SetCommandInfoRepository(CommandInfoRepository repository)
        {
            var httpExecutorProperty = this.CommandExecutor.GetType().GetProperty("HttpExecutor");
            var executor = httpExecutorProperty.GetValue(this.CommandExecutor);
            var commandInfoRepositoryField = executor.GetType().GetField("commandInfoRepository", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            commandInfoRepositoryField.SetValue(executor, repository);
        }
    }
}
