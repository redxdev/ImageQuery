using System;
using ImageQuery.Environment;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Expressions
{
    public class TernaryExpression : IExpression
    {
        public IExpression Condition { get; set; }
        public IExpression True { get; set; }
        public IExpression False { get; set; }

        public IQueryValue Evaluate(IEnvironment env)
        {
            if (Condition.Evaluate(env).Boolean)
            {
                return True.Evaluate(env);
            }
            else
            {
                return False.Evaluate(env);
            }
        }
    }
}
