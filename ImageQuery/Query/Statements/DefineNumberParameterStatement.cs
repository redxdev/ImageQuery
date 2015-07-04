using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Environment;
using ImageQuery.Query.Expressions;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Statements
{
    public class DefineNumberParameterStatement : IQueryStatement
    {
        public string Name { get; set; }
        public IExpression Value { get; set; }

        public void Run(IEnvironment env)
        {
            IQueryValue value = null;
            if (!env.HasParameter(Name))
            {
                if (Value == null)
                    throw new KeyNotFoundException(string.Format("Parameter \"{0}\" was not defined", Name));

                value = Value.Evaluate(env);
            }
            else
            {
                value = env.GetParameter(Name);
            }

            if (value.GetIQLType() != IQLType.Number)
                throw new InvalidOperationException(string.Format("Parameter \"{0}\" must be a number", Name));

            env.CreateVariable(Name, value);
        }
    }
}
