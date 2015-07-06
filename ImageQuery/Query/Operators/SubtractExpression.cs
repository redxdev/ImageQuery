using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Canvas;
using ImageQuery.Environment;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Operators
{
    public class SubtractExpression : AbstractOperatorExpression
    {
        public SubtractExpression()
        {
            AllowDisparateTypes = true;
        }

        public override IQueryValue EvaluateOperator(IEnvironment env, IQueryValue left, IQueryValue right)
        {
            switch (left.GetIQLType())
            {
                default:
                    throw new ArgumentException(string.Format("Left side of - cannot be of type {0}", left.GetIQLType()));

                case IQLType.Number:
                    switch (right.GetIQLType())
                    {
                        default:
                            throw new ArgumentException(string.Format("Right side of - cannot be of type {0} when left side is a number", right.GetIQLType()));

                        case IQLType.Number:
                            return new NumberValue() { Number = left.Number - right.Number };

                        case IQLType.Color:
                            return new ColorValue() { Color = right.Color.Each(v => left.Number - v) };
                    }

                case IQLType.Color:
                    switch (right.GetIQLType())
                    {
                        default:
                            throw new ArgumentException(string.Format("Right side of - cannot be of type {0} when left side is a color", right.GetIQLType()));

                        case IQLType.Color:
                            return new ColorValue() { Color = left.Color - right.Color };

                        case IQLType.Number:
                            return new ColorValue() { Color = left.Color.Each(v => v - right.Number) };
                    }
            }
        }
    }
}
