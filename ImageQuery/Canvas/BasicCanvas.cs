using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageQuery.Canvas
{
    public class BasicCanvas : ICanvas
    {
        public BasicCanvas(CanvasMode mode, string name, int w, int h)
        {
            if(w < 0)
                throw new ArgumentException("Width cannot be < 0", "w");

            if(h < 0)
                throw new ArgumentException("Height cannot be < 0", "h");

            Mode = mode;
            Name = name;
            Width = w;
            Height = h;

            _data = new Color[w, h];
            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    _data[x, y] = new Color();
                }
            }
        }

        public CanvasMode Mode { get; private set; }

        public string Name { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        private Color[,] _data = null;

        public Color this[int x, int y]
        {
            get
            {
                if (!Mode.CanRead())
                    throw new AccessViolationException(string.Format("cannot read canvas when marked as {0}", Mode.ToString()));

                if (x < 0 || x > Width)
                    throw new IndexOutOfRangeException(string.Format("x value ({0}) is out of range (width = {1})", x, Width));

                if(y < 0 || y > Height)
                    throw new IndexOutOfRangeException(string.Format("y value ({0}) is out of range (height = {1})", y, Height));

                return _data[x, y];
            }
            set
            {
                if(!Mode.CanWrite())
                    throw new AccessViolationException(string.Format("cannot write canvas when marked as {0}", Mode.ToString()));

                if (x < 0 || x > Width)
                    throw new IndexOutOfRangeException(string.Format("x value ({0}) is out of range (width = {1})", x, Width));

                if (y < 0 || y > Height)
                    throw new IndexOutOfRangeException(string.Format("y value ({0}) is out of range (height = {1})", y, Height));

                _data[x, y] = value;
            }
        }

        public void ForceWrite(int x, int y, Color color)
        {
            _data[x, y] = color;
        }

        public Color ForceRead(int x, int y)
        {
            return _data[x, y];
        }

        public override string ToString()
        {
            string dataStr = string.Empty;
            foreach (Color color in _data)
            {
                dataStr += color.ToString() + " ";
            }

            return string.Format("{0}[{1},{2}] {3}", Name, Width, Height, dataStr);
        }
    }
}
