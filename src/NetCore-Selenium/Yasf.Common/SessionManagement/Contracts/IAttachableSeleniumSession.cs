using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Yasf.Common.SessionManagement.Contracts
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
        /// The full response persisted by this Framework: preserves the likes of the debuggerURL etc. 
        /// </summary>
        Response Response { get; }

        /// <summary>
        /// The Response persisted when the session was originally created. This object is correctly persisted and deserialized using the
        /// in-built ToJson and FromJson.
        /// </summary>
        public string OfficialResponse { get; }

        /// <summary>
        /// URL of the RemoteServerUri (typically: localhost:someport). 
        /// </summary>
        string RemoteServerUri { get; }

        /// <summary>
        /// The name of the CommandRepository. This is used to determine the specification compliance in Selenium (so we need it)
        /// </summary>
        string CommandRepositoryTypeName { get; }
    }
}
