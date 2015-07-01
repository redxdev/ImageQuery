using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageQuery.Canvas
{
    public class BasicCanvas : ICanvas
    {
        public CanvasMode Mode { get; private set; }

        string Name { get; private set; }

        public uint Width { get; private set; }

        public uint Height { get; private set; }

        public Color this[uint x, uint y]
        {
            get
            {
                if (!Mode.CanRead())
                    throw new AccessViolationException(string.Format("cannot read canvas when marked as {0}", Mode.ToString()));

                if (x < 0 || x > Width)
                    throw new IndexOutOfRangeException(string.Format("x value ({0}) is out of range (width = {1})", x, Width));

                if(y < 0 || y > Height)
                    throw new IndexOutOfRangeException(string.Format("y value ({0}) is out of range (height = {1})", y, Height));

                return Data[x, y];
            }
            set
            {
                if(!Mode.CanWrite())
                    throw new AccessViolationException(string.Format("cannot write canvas when marked as {0}", Mode.ToString()));

                if (x < 0 || x > Width)
                    throw new IndexOutOfRangeException(string.Format("x value ({0}) is out of range (width = {1})", x, Width));

                if (y < 0 || y > Height)
                    throw new IndexOutOfRangeException(string.Format("y value ({0}) is out of range (height = {1})", y, Height));

                Data[x, y] = value;
            }
        }

        private Color[,] Data = null;

        public BasicCanvas(CanvasMode mode, string name, uint w, uint h)
        {
            Mode = mode;
            Name = name;
            Data = new Color[w, h];
            Width = w;
            Height = h;
        }
    }
}
