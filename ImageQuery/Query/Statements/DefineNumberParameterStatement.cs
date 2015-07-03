using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Environment;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Statements
{
    public class DefineNumberParameterStatement : IQueryStatement
    {
        public string Name { get; set; }

        public void Run(IEnvironment env)
        {
            IQueryValue value = env.GetParameter(Name);
            if(value.GetIQLType() != IQLType.Number)
                throw new InvalidOperationException(string.Format("Parameter \"{0}\" must be a number", Name));

            env.CreateVariable(Name, value);
        }
    }
}
