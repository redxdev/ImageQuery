using ImageQuery.Canvas;
using ImageQuery.Environment;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Expressions
{
    public class ColorExpression : IExpression
    {
        public IExpression R { get; set; }
        public IExpression G { get; set; }
        public IExpression B { get; set; }
        public IExpression A { get; set; }

        public IQueryValue Evaluate(IEnvironment env)
        {
            float r = R == null ? 0 : R.Evaluate(env).Number;
            float g = G == null ? r : G.Evaluate(env).Number;
            float b = B == null ? g : B.Evaluate(env).Number;
            float a = A == null ? 1 : A.Evaluate(env).Number;

            return new ColorValue() {Color = new Color(r, g, b, a)};
        }
    }
}
