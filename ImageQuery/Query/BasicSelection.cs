using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Canvas;
using ImageQuery.Environment;
using ImageQuery.Query.Expressions;

namespace ImageQuery.Query
{
    public class BasicSelection : ISelection
    {
        public string CanvasName { get; set; }

        public IExpression Manipulation { get; set; }

        public IExpression Where { get; set; }

        public event UnitCompleted OnUnitCompleted;

        private LinkedList<Unit> _units = null;

        public void Execute(IEnvironment env, bool storeResults)
        {
            if (Manipulation == null)
            {
                throw new ArgumentException("Manipulation expression cannot be null", "Modulation");
            }

            if (CanvasName == null)
            {
                throw new ArgumentException("Canvas name cannot be null", "CanvasName");
            }

            ICanvas canvas = env.GetVariable(CanvasName).Canvas;

            if (storeResults)
            {
                _units = new LinkedList<Unit>();
            }
            
            CanvasSelectionEnvironment canvasEnv = new CanvasSelectionEnvironment(env, canvas);
            for (int y = 0; y < canvas.Height; ++y)
            {
                for (int x = 0; x < canvas.Width; ++x)
                {
                    canvasEnv.X = x;
                    canvasEnv.Y = y;

                    if (Where != null)
                    {
                        if (!Where.Evaluate(canvasEnv).Boolean)
                            continue;
                    }

                    Unit unit = new Unit
                    {
                        X = x,
                        Y = y,
                        Color = Manipulation.Evaluate(canvasEnv).Color
                    };

                    if (OnUnitCompleted != null)
                    {
                        OnUnitCompleted(unit);
                    }

                    if (storeResults)
                    {
                        _units.AddLast(unit);
                    }
                }
            }
        }

        public Unit[] Results()
        {
            if (_units == null)
            {
                throw new InvalidOperationException("Cannot retrieve selection results before executing the selection");
            }

            return _units.ToArray();
        }
    }
}
