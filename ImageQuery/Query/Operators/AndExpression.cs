using System;
using ImageQuery.Environment;
using ImageQuery.Query.Expressions;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Operators
{
    public class AndExpression : IExpression
    {
        public IExpression Left { get; set; }
        public IExpression Right { get; set; }

        public IQueryValue Evaluate(IEnvironment env)
        {
            IQueryValue left = Left.Evaluate(env);
            return new BooleanValue() { Boolean = Left.Evaluate(env).Boolean && Right.Evaluate(env).Boolean };
        }
    }
}