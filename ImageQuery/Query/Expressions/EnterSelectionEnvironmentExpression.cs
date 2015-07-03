using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Environment;
using ImageQuery.Query.Value;

namespace ImageQuery.Query.Expressions
{
    public class EnterSelectionEnvironmentExpression : IExpression
    {
        public string Name { get; set; }

        public IExpression X { get; set; }
        public IExpression Y { get; set; }

        public IExpression Subexpression { get; set; }

        public IQueryValue Evaluate(IEnvironment env)
        {
            IQueryValue value = env.GetVariable(Name);
            switch (value.GetIQLType())
            {
                case IQLType.Canvas:
                    return
                        Subexpression.Evaluate(new CanvasSelectionEnvironment(env, value.Canvas)
                        {
                            X = (int) X.Evaluate(env).Number,
                            Y = (int) Y.Evaluate(env).Number
                        });

                default:
                    throw new InvalidOperationException(string.Format("Cannot create a selection context from type {0}", value.GetIQLType()));
            }
        }
    }
}
