using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ImageQuery.Canvas;
using ImageQuery.Environment;
using ImageQuery.Query.Expressions;

namespace ImageQuery.Query.Selection
{
    public class BasicSelection : ISelection
    {
        public string CanvasName { get; set; }

        public IExpression Manipulation { get; set; }

        public IExpression Where { get; set; }

        public event UnitCompleted OnUnitCompleted;

        private ConcurrentBag<Unit> _units = null; 

        private CountdownEvent _countdown = null;

        public void Execute(IEnvironment env)
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

            _units = new ConcurrentBag<Unit>();

            if (env.Settings.AllowParallel)
            {
                _countdown = new CountdownEvent(canvas.Width*canvas.Height);
            }
            
            for (int y = 0; y < canvas.Height; ++y)
            {
                for (int x = 0; x < canvas.Width; ++x)
                {
                    CanvasSelectionEnvironment canvasEnv = new CanvasSelectionEnvironment(env, canvas);
                    canvasEnv.X = x;
                    canvasEnv.Y = y;

                    if (env.Settings.AllowParallel)
                    {
                        ThreadPool.QueueUserWorkItem(Worker, canvasEnv);
                    }
                    else
                    {
                        Worker(canvasEnv);
                    }
                }
            }

            if (env.Settings.AllowParallel)
            {
                _countdown.Wait();
                _countdown.Dispose();
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

        private void Worker(object obj)
        {
            try
            {
                CanvasSelectionEnvironment env = obj as CanvasSelectionEnvironment;

                if (Where != null && !Where.Evaluate(env).Boolean)
                    return;

                Unit unit = new Unit()
                {
                    X = env.X,
                    Y = env.Y,
                    Color = Manipulation.Evaluate(env).Color
                };

                if(OnUnitCompleted != null)
                    OnUnitCompleted(unit);

                _units.Add(unit);
            }
            finally
            {
                if(_countdown != null)
                    _countdown.Signal();
            }
        }
    }
}
