using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Environment;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Expressions
{
    public class FunctionCallExpression : IExpression
    {
        public string Name { get; set; }
        public IExpression[] Arguments { get; set; }

        public IQueryValue Evaluate(IEnvironment env)
        {
            QueryFunction func = env.GetFunction(Name);
            LinkedList<IQueryValue> arguments = new LinkedList<IQueryValue>();
            foreach (var expr in Arguments)
            {
                arguments.AddLast(expr.Evaluate(env));
            }

            return func(env, arguments.ToArray());
        }
    }
}
