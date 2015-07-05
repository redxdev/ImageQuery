using ImageQuery.Environment;
using ImageQuery.Query.Expressions;

namespace ImageQuery.Query.Statements
{
    public class ConditionalStatement : IQueryStatement
    {
        public IExpression Condition { get; set; }

        public IQueryStatement[] True { get; set; }

        public ElseIfSection[] ElseIf { get; set; }

        public IQueryStatement[] False { get; set; }

        public void Run(IEnvironment env)
        {
            if (Condition.Evaluate(env).Boolean)
            {
                if (True != null)
                {
                    foreach (var stm in True)
                    {
                        stm.Run(env);
                    }
                }
            }
            else if (ElseIf != null)
            {
                foreach (var section in ElseIf)
                {
                    if (section.Condition.Evaluate(env).Boolean)
                    {
                        if (section.True != null)
                        {
                            foreach (var stm in section.True)
                            {
                                stm.Run(env);
                            }

                            break;
                        }
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

        public struct ElseIfSection
        {
            public IExpression Condition { get; set; }
            public IQueryStatement[] True { get; set; }
        }
    }
}
