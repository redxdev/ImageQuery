using System;
using ImageQuery.Environment;
using ImageQuery.Query.Expressions;

namespace ImageQuery.Query.Statements
{
    public class ExpressionStatement : IQueryStatement
    {
        public IExpression Expression { get; set; }

        public void Run(IEnvironment env)
        {
            Expression.Evaluate(env);
        }
    }
}
