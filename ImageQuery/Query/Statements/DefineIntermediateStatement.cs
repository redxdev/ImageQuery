using ImageQuery.Environment;
using ImageQuery.Query.Expressions;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Statements
{
    public class DefineIntermediateStatement : IQueryStatement
    {
        public string Name { get; set; }
        public IExpression W { get; set; }
        public IExpression H { get; set; }
        public IExpression Value { get; set; }

        public void Run(IEnvironment env)
        {
            if (Value == null)
            {
                env.CreateIntermediate(Name, (int)W.Evaluate(env).Number, (int)H.Evaluate(env).Number);
            }
            else
            {
                IQueryValue value = new CanvasValue() {Canvas = Value.Evaluate(env).Canvas};
                env.CreateVariable(Name, value);
            }
        }
    }
}
