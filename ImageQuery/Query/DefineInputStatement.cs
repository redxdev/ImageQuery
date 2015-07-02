using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Environment;

namespace ImageQuery.Query
{
    public class DefineInputStatement : IQueryStatement
    {
        public string CanvasName { get; set; }

        public void Run(IEnvironment env)
        {
            env.CreateInput(CanvasName);
        }
    }
}
