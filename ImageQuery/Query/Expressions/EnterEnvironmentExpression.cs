using ImageQuery.Canvas;
using ImageQuery.Environment;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Expressions
{
    public class EnterEnvironmentExpression : IExpression
    {
        public string CanvasName { get; set; }

        public IExpression Subexpression { get; set; }

        public IQueryValue Evaluate(IEnvironment env)
        {
            ICanvas canvas = env.GetVariable(CanvasName).Canvas;
            CanvasEnvironment canvasEnv = new CanvasEnvironment(env, canvas);
            return Subexpression.Evaluate(canvasEnv);
        }
    }
}
