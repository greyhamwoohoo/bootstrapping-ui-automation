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
            if(seleniumVersion != "0.0.0.0")
            {
                // The Alpha version has nothing stamped in their DLL. So we have to guess. 
                throw new NotSupportedException($"The implementation below is very specific to the V4.0 Alpha-03 version (but we have no exact way of detecting it from the assembly version)");
            }

            //
            // NOTE: *ALL* of the following code requires Selenium 4.0 to work (tested on: Alpha 03)
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

                // The CommandInfoRepository indicates the 'specifical level' of the session. 
                //   In 4.0+, the WebDriverWireProtocolCommandInfoRepository type is no more: there appears to be only one kind of CommandInfoRepository
                //   on the code base which is W3CWireProtocolCommandInfoRepository
                if (existingSession.CommandRepositoryTypeName == typeof(W3CWireProtocolCommandInfoRepository).FullName)
                {
                    SetCommandInfoRepository(new W3CWireProtocolCommandInfoRepository());
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
