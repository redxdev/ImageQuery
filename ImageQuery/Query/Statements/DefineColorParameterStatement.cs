using System;
using System.Collections.Generic;
using ImageQuery.Environment;
using ImageQuery.Query.Expressions;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Statements
{
    public class DefineColorParameterStatement : IQueryStatement
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

            if(value.GetIQLType() != IQLType.Color)
                throw new InvalidOperationException(string.Format("Parameter \"{0}\" must be a color", Name));

            env.CreateVariable(Name, value);
        }
    }
}