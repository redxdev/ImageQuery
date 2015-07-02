using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Environment;
using ImageQuery.Query.Value;

namespace ImageQuery.Query
{
    public class RetrieveIndexedVariableExpression : IExpression
    {
        public string Name { get; set; }
        public IExpression X { get; set; }
        public IExpression Y { get; set; }

        public IQueryValue Evaluate(IEnvironment env)
        {
            IQueryValue value = env.GetVariable(Name);
            return value.Index(X.Evaluate(env), Y == null ? null : Y.Evaluate(env));
        }
    }
}
