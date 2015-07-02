using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Canvas;
using ImageQuery.Environment;

namespace ImageQuery.Query
{
    public class ApplyStatement : IQueryStatement
    {
        public string CanvasName { get; set; }

        public ISelection Selection { get; set; }

        public void Run(IEnvironment env)
        {
            ICanvas canvas = env.GetVariable(CanvasName).Canvas;
            Selection.Execute(env, true);

            Unit[] units = Selection.Results();
            foreach (var unit in units)
            {
                canvas[unit.X, unit.Y] = unit.Color;
            }
        }
    }
}
