using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Environment;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Operators
{
    public abstract class AbstractOperatorExpression : IExpression
    {
        public AbstractOperatorExpression()
        {
            AllowDisparateTypes = false;
        }

        public IExpression Left { get; set; }
        public IExpression Right { get; set; }

        protected bool AllowDisparateTypes { private get; set; }

        public IQueryValue Evaluate(IEnvironment env)
        {
            IQueryValue left = Left.Evaluate(env);
            IQueryValue right = Right.Evaluate(env);

            if (!AllowDisparateTypes && left.GetIQLType() != right.GetIQLType())
            {
                throw new ArgumentException(string.Format("Cannot perform operation on disparate types"));
            }

            return EvaluateOperator(env, left, right);
        }

        public abstract IQueryValue EvaluateOperator(IEnvironment env, IQueryValue left, IQueryValue right);
    }
}
