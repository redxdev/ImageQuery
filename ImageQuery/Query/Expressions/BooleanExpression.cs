using ImageQuery.Environment;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Expressions
{
    public class BooleanExpression : IExpression
    {
        public bool Value { get; set; }

        public IQueryValue Evaluate(IEnvironment env)
        {
            return new BooleanValue() {Boolean = Value};
        }
    }
}
