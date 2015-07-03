using ImageQuery.Environment;
using ImageQuery.Query.Expressions;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Statements
{
    public class SetVariableStatement : IQueryStatement
    {
        public string Name { get; set; }
        public IExpression Value { get; set; }

        public void Run(IEnvironment env)
        {
            IQueryValue variable = env.GetVariable(Name);
            variable.SetValue(Value.Evaluate(env));
        }
    }
}
