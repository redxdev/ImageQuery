using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageQuery.Canvas
{
    public class BitmapCanvasLoader : ICanvasLoader
    {
        private Dictionary<string, string> names = new Dictionary<string, string>();

        public void RegisterName(string name, string path)
        {
            names.Add(name, path);
        }

        public ICanvas LoadCanvas(string name)
        {
            string path = null;
            if (!names.TryGetValue(name, out path))
                return null;

            Bitmap bitmap = new Bitmap(path);
            ICanvas canvas = new BasicCanvas(CanvasMode.ReadOnly, name, bitmap.Width, bitmap.Height);

            for (int y = 0; y < canvas.Height; ++y)
            {
                for (int x = 0; x < canvas.Width; ++x)
                {
                    System.Drawing.Color color = bitmap.GetPixel(x, y);
                    canvas.ForceWrite(x, y, new Color(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f));
                }
            }

            return canvas;
        }
    }
}
