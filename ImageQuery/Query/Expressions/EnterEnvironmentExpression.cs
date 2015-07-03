using System;
using ImageQuery.Canvas;
using ImageQuery.Environment;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Expressions
{
    public class EnterEnvironmentExpression : IExpression
    {
        public string Name { get; set; }

        public IExpression Subexpression { get; set; }

        public IQueryValue Evaluate(IEnvironment env)
        {
            IQueryValue value = env.GetVariable(Name);
            switch (value.GetIQLType())
            {
                case IQLType.Canvas:
                    return Subexpression.Evaluate(new CanvasEnvironment(env, value.Canvas));

                case IQLType.Color:
                    return Subexpression.Evaluate(new ColorEnvironment(env, value.Color));

                default:
                    throw new InvalidOperationException(string.Format("Cannot create a context from type {0}", value.GetIQLType()));
            }
        }
    }
}
