using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageQuery.Canvas
{
    public static class CanvasExtensions
    {
        public static void WriteToFile(this ICanvas canvas, string filename)
        {
            Bitmap bitmap = new Bitmap(canvas.Width, canvas.Height);
            for (int y = 0; y < canvas.Height; ++y)
            {
                for (int x = 0; x < canvas.Width; ++x)
                {
                    Color color = canvas.ForceRead(x, y);
                    bitmap.SetPixel(x, y,
                        System.Drawing.Color.FromArgb((int) (color.A*255), (int) (color.R*255), (int) (color.G*255),
                            (int) (color.B*255)));
                }
            }

            bitmap.Save(filename);
        }
    }
}
