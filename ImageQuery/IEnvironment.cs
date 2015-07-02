using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Canvas;

namespace ImageQuery
{
    public interface IEnvironment
    {
        IQLSettings Settings { get; }

        ICanvas CreateInput(string name);

        ICanvas CreateOutput(string name, int w, int h);

        ICanvas CreateIntermediate(string name, int w, int h);

        void CreateVariable(string name, IQueryValue value);

        void SetVariable(string name, IQueryValue value);

        IQueryValue GetVariable(string name);
    }
}
