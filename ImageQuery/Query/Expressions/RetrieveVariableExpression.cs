using ImageQuery.Environment;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Expressions
{
    public class RetrieveVariableExpression : IExpression
    {
        public string Name { get; set; }

        public IQueryValue Evaluate(IEnvironment env)
        {
            return env.GetVariable(Name);
        }
    }
}
