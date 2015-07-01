using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ImageQuery.Canvas
{
    public interface ICanvas
    {
        CanvasMode Mode { get; }

        string Name { get; set; }

        uint Width { get; }
        uint Height { get; }

        Color this[uint x, uint y] { get; set; }
    }
}
