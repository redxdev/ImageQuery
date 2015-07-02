using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Environment;
using ImageQuery.Query.Value;

namespace ImageQuery.Query
{
    public interface IExpression
    {
        IQueryValue Evaluate(IEnvironment env);
    }
}
