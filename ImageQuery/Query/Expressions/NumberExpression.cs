using ImageQuery.Environment;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Expressions
{
    public class NumberExpression : IExpression
    {
        public float Value { get; set; }

        public IQueryValue Evaluate(IEnvironment env)
        {
            return new NumberValue() {Number = Value};
        }
    }
}
