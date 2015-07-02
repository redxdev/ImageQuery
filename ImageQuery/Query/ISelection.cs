using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Environment;

namespace ImageQuery.Query
{
    public delegate void UnitCompleted(Unit result);

    public interface ISelection
    {
        event UnitCompleted OnUnitCompleted;

        void Execute(IEnvironment env, bool storeResults);

        Unit[] Results();
    }
}
