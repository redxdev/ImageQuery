using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Canvas;

namespace ImageQuery.Query.Operators
{
    public class SubtractExpression : AbstractOperator
    {
        public override IQueryValue EvaluateOperator(IEnvironment env, IQueryValue left, IQueryValue right)
        {
            switch (left.GetIQLType())
            {
                default:
                    throw new ArgumentException(string.Format("Left side of - cannot be of type {0}", left.GetIQLType()));

                case IQLType.Number:
                    return new NumberValue() {Number = left.Number - right.Number};

                case IQLType.Color:
                    return new ColorValue() {Color = left.Color - right.Color};
            }
        }
    }
}
