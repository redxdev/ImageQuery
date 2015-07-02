using System;
using ImageQuery.Canvas;

namespace ImageQuery.Query.Value
{
    public class BooleanValue : IQueryValue
    {
        public float Number
        {
            get
            {
                throw new InvalidOperationException("Trying to use a boolean as a number");
            }

            set
            {
                throw new InvalidOperationException("Trying to use a boolean as a number");
            }
        }

        public Color Color
        {
            get
            {
                throw new InvalidOperationException("Trying to use a boolean as a color");
            }

            set
            {
                throw new InvalidOperationException("Trying to use a boolean as a color");
            }
        }

        public ICanvas Canvas
        {
            get
            {
                throw new InvalidOperationException("Trying to use a boolean as a canvas");
            }

            set
            {
                throw new InvalidOperationException("Trying to use a boolean as a canvas");
            }
        }

        public bool Boolean { get; set; }
        public IQLType GetIQLType()
        {
            return IQLType.Boolean;
        }

        public IQueryValue Index(IQueryValue x, IQueryValue y)
        {
            throw new InvalidOperationException("Cannot index object of type boolean");
        }
    }
}
