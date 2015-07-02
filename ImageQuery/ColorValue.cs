using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Canvas;

namespace ImageQuery
{
    public class ColorValue : IQueryValue
    {
        public float Number
        {
            get
            {
                throw new InvalidOperationException("Trying to use a color as a number");
            }

            set
            {
                throw new InvalidOperationException("Trying to use a color as a number");
            }
        }

        public Color Color { get; set; }

        public ICanvas Canvas
        {
            get
            {
                throw new InvalidOperationException("Trying to use a color as a canvas");
            }

            set
            {
                throw new InvalidOperationException("Trying to use a color as a canvas");
            }
        }

        public bool Boolean
        {
            get
            {
                throw new InvalidOperationException("Trying to use a color as a boolean");
            }

            set
            {
                throw new InvalidOperationException("Trying to use a color as a boolean");
            }
        }

        public IQLType GetIQLType()
        {
            return IQLType.Color;
        }
    }
}
