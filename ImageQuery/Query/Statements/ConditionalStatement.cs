using ImageQuery.Environment;
using ImageQuery.Query.Expressions;

namespace ImageQuery.Query.Statements
{
    public class ConditionalStatement : IQueryStatement
    {
        public IExpression Condition { get; set; }

        public IQueryStatement[] True { get; set; }

        public IQueryStatement[] False { get; set; }

        public void Run(IEnvironment env)
        {
            if (Condition.Evaluate(env).Boolean)
            {
                if (true != null)
                {
                    foreach (var stm in True)
                    {
                        stm.Run(env);
                    }
                }
            }
            else if (False != null)
            {
                foreach (var stm in False)
                {
                    stm.Run(env);
                }
            }
        }
    }
}
