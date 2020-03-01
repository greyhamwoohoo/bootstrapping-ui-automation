using OpenQA.Selenium.Remote;

namespace TheInternet.Common.SessionManagement.Contracts
{
    /// <summary>
    /// Represents read only information for an attachable Selenium Session
    /// </summary>
    public interface IAttachableSeleniumSession
    {
        /// <summary>
        /// Returns true if this object is valid; false otherwise. 
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// The Browser Name
        /// </summary>
        string BrowserName { get; }

        /// <summary>
        /// The Response persisted when the session was originally created. 
        /// </summary>
        Response Response { get; }

        /// <summary>
        /// URL of the RemoteServerUri (typically: localhost:someport). 
        /// </summary>
        string RemoteServerUri { get; }
    }
}
