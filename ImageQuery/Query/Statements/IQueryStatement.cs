using ImageQuery.Environment;

namespace ImageQuery.Query.Statements
{
    public interface IQueryStatement
    {
        void Run(IEnvironment env);
    }
}
