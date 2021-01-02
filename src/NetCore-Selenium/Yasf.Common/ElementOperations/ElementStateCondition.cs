using OpenQA.Selenium;

namespace Yasf.Common.ElementOperations
{
    /// <summary>
    /// Provides a fluent interface around the expected state of multiple elements. 
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class ElementStateCondition
    {
        public By Locator { get; set; }
        public ElementState State { get; set; }

        public ElementStateCondition()
        {
        }

        public static ElementStateCondition And(By element, ElementState state)
        {
            return new ElementStateCondition() { Locator = element, State = state };
        }

        public override string ToString()
        {
            return $"{Locator.ToString()},{State.ToString()}";
        }
    }
}
