using ImageQuery.Environment;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Operators
{
    public class InequalityExpression : AbstractOperatorExpression
    {
        public InequalityExpression()
        {
            AllowDisparateTypes = true;
        }

        public override IQueryValue EvaluateOperator(IEnvironment env, IQueryValue left, IQueryValue right)
        {
            if (left.GetIQLType() != right.GetIQLType())
                return new BooleanValue() {Boolean = false};

            switch (left.GetIQLType())
            {
                case IQLType.Number:
                    return new BooleanValue() {Boolean = left.Number != right.Number};

                case IQLType.Color:
                    return new BooleanValue() {Boolean = !left.Color.Equals(right.Color)};

                case IQLType.Canvas:
                    return new BooleanValue() {Boolean = left.Canvas != right.Canvas};

                case IQLType.Boolean:
                    return new BooleanValue() {Boolean = left.Boolean != right.Boolean};
            }

            return new BooleanValue() {Boolean = false};
        }
    }
}