using System;
using ImageQuery.Environment;
using ImageQuery.Library;
using ImageQuery.Query.Expressions;

namespace ImageQuery.Query.Statements
{
    public class DefineFunctionStatement : IQueryStatement
    {
        public string Name { get; set; }

        public string[] ArgumentNames { get; set; }

        public IQueryStatement[] Statements { get; set; }

        public IExpression FinalExpression { get; set; }

        public void Run(IEnvironment env)
        {
            QueryFunction func = (parent, args) =>
            {
                FunctionUtils.CheckArgumentCountEqual(Name, ArgumentNames.Length, args.Length);

                var fenv = new FunctionEnvironment(parent);
                for (int i = 0; i < ArgumentNames.Length; ++i)
                {
                    fenv.CreateVariable(ArgumentNames[i], args[i]);
                }

                if (Statements != null)
                {
                    foreach (var stm in Statements)
                    {
                        stm.Run(fenv);
                    }
                }

                return FinalExpression.Evaluate(fenv);
            };

            env.RegisterFunction(Name, func);
        }
    }
}
