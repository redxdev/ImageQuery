using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Environment;
using ImageQuery.Query.Value;

namespace ImageQuery.Query
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
