using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageQuery.Canvas
{
    public class DelegatedCanvasLoader : ICanvasLoader
    {
        public DelegatedCanvasLoader(Func<string, ICanvas> func)
        {
            _func = func;
        }

        private Func<string, ICanvas> _func = null; 

        public ICanvas LoadCanvas(string name)
        {
            return _func(name);
        }
    }
}
