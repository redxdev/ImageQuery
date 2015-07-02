using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Environment;

namespace ImageQuery.Query
{
    public class DefineCanvasStatement : IQueryStatement
    {
        public string CanvasName { get; set; }
        public int W { get; set; }
        public int H { get; set; }

        public void Run(IEnvironment env)
        {
            env.CreateIntermediate(CanvasName, W, H);
        }
    }
}
