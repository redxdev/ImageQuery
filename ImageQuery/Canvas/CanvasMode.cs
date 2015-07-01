using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageQuery.Canvas
{
    public enum CanvasMode
    {
        Locked,
        ReadOnly,
        WriteOnly,
        ReadWrite
    }

    public static class CanvasModeExtensions
    {
        public static bool CanRead(this CanvasMode mode)
        {
            return mode == CanvasMode.ReadOnly || mode == CanvasMode.ReadWrite;
        }

        public static bool CanWrite(this CanvasMode mode)
        {
            return mode == CanvasMode.WriteOnly || mode == CanvasMode.ReadWrite;
        }
    }
}
