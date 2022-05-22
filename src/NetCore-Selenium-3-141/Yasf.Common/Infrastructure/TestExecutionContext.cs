using System.Collections.Generic;
using System.Text;

namespace Yasf.Common.Infrastructure
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
