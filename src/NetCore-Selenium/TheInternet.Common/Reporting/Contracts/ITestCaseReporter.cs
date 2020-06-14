namespace TheInternet.Common.Reporting.Contracts
{
    public interface ITestCaseReporter
    {
        void Initialize(string name);
        void Uninitialize();
        string Name { get; }
    }
}
