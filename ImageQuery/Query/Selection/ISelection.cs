using ImageQuery.Environment;

namespace ImageQuery.Query.Selection
{
    public delegate void UnitCompleted(Unit result);

    public interface ISelection
    {
        event UnitCompleted OnUnitCompleted;

        void Execute(IEnvironment env);

        Unit[] Results();
    }
}
