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
    /// If the chrome driver instance is NOT running, then this will simply start a session as normal.
    /// If the chrome driver instance IS running, then this will attach to the running instance. 
    /// TODO: Move the persistence/checking out of this driver. 
    /// TODO: The constructor of the WebDrivers initiates session creation: so we are limited at what we can 'inject'
    /// before the connection process starts. As a result, I need to new up everything here instead of using DI. 
    /// </remarks>
    public class AttachableChromeDriver : ChromeDriver
    {
        public AttachableChromeDriver(ChromeOptions options) : base(options)
        {
        }

        protected override Response Execute(string driverCommandToExecute, Dictionary<string, object> parameters)
        {
            if (driverCommandToExecute == "newSession")
            {
                var attachableSeleniumSessionStorage = new AttachableSeleniumSessionStorage(Directory.GetCurrentDirectory());
                var existingSession = attachableSeleniumSessionStorage.ReadSessionState();
                var sessionProbe = new AttachableSeleniumSessionProbe();

                if(!existingSession.IsValid || existingSession.BrowserName != "CHROME" || !sessionProbe.IsRunning(existingSession))
                {
                    attachableSeleniumSessionStorage.RemoveSessionState();
                    existingSession = attachableSeleniumSessionStorage.ReadSessionState();
                }

                if(!existingSession.IsValid)
                {
                    // There is currently no persisted session we can use. 
                    var newSession = base.Execute(driverCommandToExecute, parameters);

                    var attachableSeleniumSession = new AttachableSeleniumSession()
                    {
                        BrowserName = "CHROME",
                        Response = newSession,
                        RemoteServerUri = GetRemoteServerUri()
                    };

                    attachableSeleniumSessionStorage.WriteSessionState(attachableSeleniumSession);

                    return newSession;
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
    }
}
