using System;
using ImageQuery.Environment;
using ImageQuery.Query.Expressions;

namespace ImageQuery.Query.Statements
{
    public class WhileStatement : IQueryStatement
    {
        public IExpression Condition { get; set; }

        public IQueryStatement[] Statements { get; set; }

        public void Run(IEnvironment env)
        {
            while (Condition.Evaluate(env).Boolean)
            {
                if (Statements != null)
                {
                    foreach (var stm in Statements)
                    {
                        stm.Run(env);
                    }
                }
            }
        }
    }
}
