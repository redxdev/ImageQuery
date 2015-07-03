using ImageQuery.Canvas;
using ImageQuery.Environment;
using ImageQuery.Query.Expressions;

namespace ImageQuery.Query.Statements
{
    public class ApplyStatement : IQueryStatement
    {
        public string CanvasName { get; set; }

        public ISelection Selection { get; set; }

        public IExpression XManipulation { get; set; }
        public IExpression YManipulation { get; set; }

        public void Run(IEnvironment env)
        {
            ICanvas canvas = env.GetVariable(CanvasName).Canvas;
            Selection.Execute(env, true);

            Unit[] units = Selection.Results();
            CanvasSelectionEnvironment canvasEnv = new CanvasSelectionEnvironment(env, canvas);

            foreach (var unit in units)
            {
                canvasEnv.X = unit.X;
                canvasEnv.Y = unit.Y;
                int x = XManipulation == null ? unit.X : (int) XManipulation.Evaluate(canvasEnv).Number;
                int y = YManipulation == null ? unit.Y : (int) YManipulation.Evaluate(canvasEnv).Number;
                canvas[x, y] = unit.Color;
            }
        }
    }
}
