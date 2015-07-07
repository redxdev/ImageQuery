using System;
using ImageQuery.Environment;
using ImageQuery.Query.Expressions;

namespace ImageQuery.Query.Statements
{
    public class DoWhileStatement : IQueryStatement
    {
        public IExpression Condition { get; set; }
        public IQueryStatement[] Statements { get; set; }

        public void Run(IEnvironment env)
        {
            do
            {
                if (Statements != null)
                {
                    foreach (var stm in Statements)
                    {
                        stm.Run(env);
                    }
                }
            } while (Condition.Evaluate(env).Boolean);
        }
    }
}
