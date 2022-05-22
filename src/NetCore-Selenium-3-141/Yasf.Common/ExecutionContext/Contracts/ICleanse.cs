namespace Yasf.Common.ExecutionContext.Contracts
{
    /// <summary>
    /// Helper to make the object 'good to go'. Preference is to use a Json or Xml deserialization hook, but they differ slightly in semantics. 
    /// </summary>
    public interface ICleanse
    {
        void Cleanse();
    }
}
