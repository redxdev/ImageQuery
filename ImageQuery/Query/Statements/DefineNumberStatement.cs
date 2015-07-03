using ImageQuery.Environment;
using ImageQuery.Query.Expressions;

namespace ImageQuery.Query.Statements
{
    public class DefineNumberStatement : IQueryStatement
    {
        public string Name { get; set; }
        public IExpression Value { get; set; }

        public void Run(IEnvironment env)
        {
            var variable = new NumberValue();
            env.CreateVariable(Name, variable);

            if (Value != null)
            {
                variable.Number = Value.Evaluate(env).Number;
            }
        }
    }
}
