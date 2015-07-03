using System;
using ImageQuery.Environment;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Statements
{
    public class DefineColorParameterStatement : IQueryStatement
    {
        public string Name { get; set; }

        public void Run(IEnvironment env)
        {
            IQueryValue value = env.GetParameter(Name);
            if(value.GetIQLType() != IQLType.Color)
                throw new InvalidOperationException(string.Format("Parameter \"{0}\" must be a color", Name));

            env.CreateVariable(Name, value);
        }
    }
}