using ImageQuery.Environment;

namespace ImageQuery.Query.Statements
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
