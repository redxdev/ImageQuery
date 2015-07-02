using System;
using ImageQuery.Environment;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Operators
{
    public class LessThanOrEqualExpression : AbstractOperatorExpression
    {
        public override IQueryValue EvaluateOperator(IEnvironment env, IQueryValue left, IQueryValue right)
        {
            switch (left.GetIQLType())
            {
                default:
                    throw new ArgumentException(string.Format("Arguments to <= cannot be of type {0}", left.GetIQLType()));

                case IQLType.Number:
                    return new BooleanValue() { Boolean = left.Number <= right.Number };
            }
        }
    }
}