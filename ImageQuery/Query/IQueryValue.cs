using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Canvas;

namespace ImageQuery.Query
{
    public interface IQueryValue
    {
        float GetNumber();
        void SetNumber(float value);

        Color GetColor();
        void SetColor(Color value);

        ICanvas GetCanvas();
        void SetCanvas(ICanvas value);
    }
}
