using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Environment;

namespace ImageQuery.Query
{
    interface IQueryStatement
    {
        void Run(IEnvironment env);
    }
}
