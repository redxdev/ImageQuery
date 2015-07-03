using System;

namespace ImageQuery.Canvas
{
    public class IteratorCanvas : ICanvas
    {
        public IteratorCanvas(int w, int h)
        {
            if (w < 0)
                throw new ArgumentException("Width cannot be < 0", "w");

            if (h < 0)
                throw new ArgumentException("Height cannot be < 0", "h");

            Width = w;
            Height = h;
        }

        public CanvasMode Mode { get { return CanvasMode.ReadOnly; } }
        public string Name { get { return "$";  } }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Color this[int x, int y]
        {
            get
            {
                if (x < 0 || x > Width)
                    throw new IndexOutOfRangeException(string.Format("x value ({0}) is out of range (width = {1})", x, Width));

                if (y < 0 || y > Height)
                    throw new IndexOutOfRangeException(string.Format("y value ({0}) is out of range (height = {1})", y, Height));

                return new Color();
            }
            set { throw new AccessViolationException("cannot write iterator canvas"); }
        }

        public void ForceWrite(int x, int y, Color color)
        {
            throw new AccessViolationException("cannot write iterator canvas");
        }

        public Color ForceRead(int x, int y)
        {
            return new Color();
        }
    }
}
