namespace TheInternet.Common.SessionManagement.Contracts
{
    /// <summary>
    /// Returns TRUE if it looks like a session is running on the remote port. 
    /// </summary>
    public interface IAttachableSeleniumSessionProbe
    {
        bool IsRunning(IAttachableSeleniumSession attachableSeleniumSession);
    }
}
