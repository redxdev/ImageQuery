using System;
using ImageQuery.Environment;
using ImageQuery.Query.Expressions;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Operators
{
    public class OrExpression : IExpression
    {
        public IExpression Left { get; set; }
        public IExpression Right { get; set; }

        public IQueryValue Evaluate(IEnvironment env)
        {
            return new BooleanValue() {Boolean = Left.Evaluate(env).Boolean || Right.Evaluate(env).Boolean};
        }
    }
}