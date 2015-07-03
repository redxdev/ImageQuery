using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Environment;
using ImageQuery.Query.Expressions;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Operators
{
    public class NotExpression : IExpression
    {
        public IExpression Operand { get; set; }

        public IQueryValue Evaluate(IEnvironment env)
        {
            return new BooleanValue() {Boolean = !Operand.Evaluate(env).Boolean};
        }
    }
}
