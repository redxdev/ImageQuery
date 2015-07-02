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

        string Name { get; }

        int Width { get; }
        int Height { get; }

        Color this[int x, int y] { get; set; }

        void ForceWrite(int x, int y, Color color);
        Color ForceRead(int x, int y);
    }
}
