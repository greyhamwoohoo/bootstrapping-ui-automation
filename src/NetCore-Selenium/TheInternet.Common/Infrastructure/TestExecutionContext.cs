using System.Collections.Generic;
using System.Text;

namespace TheInternet.Common.Infrastructure
{
    public class TestExecutionContext
    {
        public TestExecutionContext()
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
