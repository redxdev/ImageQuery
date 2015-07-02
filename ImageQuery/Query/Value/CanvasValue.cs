using System;
using ImageQuery.Canvas;

namespace ImageQuery.Query.Value
{
    public class CanvasValue : IQueryValue
    {
        public CanvasValue()
        {
            Canvas = null;
        }

        public float Number
        {
            get
            {
                throw new InvalidOperationException("Trying to use a canvas as a number");
            }

            set
            {
                throw new InvalidOperationException("Trying to use a canvas as a number");
            }
        }

        public Color Color
        {
            get
            {
                throw new InvalidOperationException("Trying to use a canvas as a color");
            }

            set
            {
                throw new InvalidOperationException("Trying to use a canvas as a color");
            }
        }

        public ICanvas Canvas { get; set; }

        public bool Boolean
        {
            get
            {
                throw new InvalidOperationException("Trying to use a canvas as a boolean");
            }

            set
            {
                throw new InvalidOperationException("Trying to use a canvas as a boolean");
            }
        }

        public IQLType GetIQLType()
        {
            return IQLType.Canvas;
        }

        public IQueryValue Index(IQueryValue x, IQueryValue y)
        {
            throw new InvalidOperationException("Cannot index object of type canvas");
        }
    }
}
