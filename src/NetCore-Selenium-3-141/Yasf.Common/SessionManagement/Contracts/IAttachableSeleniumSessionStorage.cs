namespace Yasf.Common.SessionManagement.Contracts
{
    public interface IAttachableSeleniumSessionStorage
    {
        bool AttachableSessionExists { get; }
        string Path { get; }
        IAttachableSeleniumSession ReadSessionState(string browserName);
        void RemoveSessionState();
        void WriteSessionState(IAttachableSeleniumSession session);
    }
}
