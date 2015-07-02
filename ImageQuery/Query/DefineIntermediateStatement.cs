using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Environment;

namespace ImageQuery.Query
{
    public class DefineIntermediateStatement : IQueryStatement
    {
        public string CanvasName { get; set; }
        public IExpression W { get; set; }
        public IExpression H { get; set; }

        public void Run(IEnvironment env)
        {
            env.CreateIntermediate(CanvasName, (int)W.Evaluate(env).Number, (int)H.Evaluate(env).Number);
        }
    }
}
