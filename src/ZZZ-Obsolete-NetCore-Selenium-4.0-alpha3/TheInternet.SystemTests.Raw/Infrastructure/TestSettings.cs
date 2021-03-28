using System.Collections.Generic;
using System.Text;

namespace TheInternet.SystemTests.Raw.Infrastructure
{
    public class TestSettings
    {
        public TestSettings()
        {
            EnvironmentVariables = new EnvironmentVariableCollection();
        }

        public EnvironmentVariableCollection EnvironmentVariables { get; set; }
    }

    public class EnvironmentVariableCollection : Dictionary<string, string>
    {
        public EnvironmentVariableCollection()
        {
        }
    }
}
