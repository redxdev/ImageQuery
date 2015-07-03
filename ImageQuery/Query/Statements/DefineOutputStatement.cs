using ImageQuery.Environment;
using ImageQuery.Query.Expressions;

namespace ImageQuery.Query.Statements
{
    public class DefineOutputStatement : IQueryStatement
    {
        public string CanvasName { get; set; }
        public IExpression W { get; set; }
        public IExpression H { get; set; }

        public void Run(IEnvironment env)
        {
            env.CreateOutput(CanvasName, (int)W.Evaluate(env).Number, (int)H.Evaluate(env).Number);
        }
    }
}
