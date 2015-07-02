using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageQuery.Query
{
    public class RetrieveVariableExpression : IExpression
    {
        public string Name { get; set; }

        public IQueryValue Evaluate(IEnvironment env)
        {
            return env.GetVariable(Name);
        }
    }
}
