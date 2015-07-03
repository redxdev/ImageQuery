using ImageQuery.Environment;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Expressions
{
    public interface IExpression
    {
        IQueryValue Evaluate(IEnvironment env);
    }
}
