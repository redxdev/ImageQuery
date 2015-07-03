using ImageQuery.Canvas;
using ImageQuery.Environment;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Expressions
{
    public class IteratorExpression : IExpression
    {
        public IExpression Width { get; set; }
        public IExpression Height { get; set; }

        public IQueryValue Evaluate(IEnvironment env)
        {
            return new CanvasValue() {Canvas = new IteratorCanvas((int)Width.Evaluate(env).Number, (int)Height.Evaluate(env).Number)};
        }
    }
}
