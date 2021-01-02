namespace Yasf.Common.ExecutionContext.Runtime.BrowserSettings.Contracts
{
    public interface IBrowserProperties
    {
        string Name { get; }
        BrowserSettingsBase BrowserSettings { get; }
    }
}
