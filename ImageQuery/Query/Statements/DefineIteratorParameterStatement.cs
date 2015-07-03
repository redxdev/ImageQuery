using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Canvas;
using ImageQuery.Environment;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Statements
{
    public class DefineIteratorParameterStatement : IQueryStatement
    {
        public string Name { get; set; }

        public void Run(IEnvironment env)
        {
            IQueryValue value = env.GetParameter(Name);
            if (value.GetIQLType() != IQLType.Canvas || !(value.Canvas is IteratorCanvas))
                throw new InvalidOperationException(string.Format("Parameter \"{0}\" must be an iterator", Name));

            env.CreateVariable(Name, value);
        }
    }
}
