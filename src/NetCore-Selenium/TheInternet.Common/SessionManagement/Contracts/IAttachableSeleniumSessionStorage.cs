namespace TheInternet.Common.SessionManagement.Contracts
{
    public interface IAttachableSeleniumSessionStorage
    {
        bool AttachableSessionExists { get; }
        string Path { get; }
        AttachableSeleniumSession ReadSessionState();
        void RemoveSessionState();
        void WriteSessionState(AttachableSeleniumSession session);
    }
}
