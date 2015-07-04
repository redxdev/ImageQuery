using ImageQuery.Canvas;
using ImageQuery.Environment;
using ImageQuery.Query.Expressions;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Statements
{
    public class DefineColorStatement : IQueryStatement
    {
        public string Name { get; set; }
        public IExpression Value { get; set; }
        public void Run(IEnvironment env)
        {
            var variable = new ColorValue();
            env.CreateVariable(Name, variable);

            if (Value != null)
            {
                variable.Color = Value.Evaluate(env).Color;
            }
            else
            {
                variable.Color = new Color(0, 0, 0, 1);
            }
        }
    }
}
