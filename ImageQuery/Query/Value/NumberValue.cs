using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Canvas;
using ImageQuery.Query.Value;

namespace ImageQuery
{
    public class NumberValue : IQueryValue
    {
        public float Number { get; set; }

        public Color Color
        {
            get
            {
                throw new InvalidOperationException("Trying to use a number as a color");
            }

            set
            {
                throw new InvalidOperationException("Trying to use a number as a color");
            }
        }

        public ICanvas Canvas
        {
            get
            {
                throw new InvalidOperationException("Trying to use a number as a canvas");
            }

            set
            {
                throw new InvalidOperationException("Trying to use a number as a canvas");
            }
        }

        public bool Boolean
        {
            get
            {
                throw new InvalidOperationException("Trying to use a number as a boolean");
            }

            set
            {
                throw new InvalidOperationException("Trying to use a number as a boolean");
            }
        }

        public IQLType GetIQLType()
        {
            return IQLType.Number;
        }

        public IQueryValue Index(IQueryValue x, IQueryValue y)
        {
            throw new InvalidOperationException("Cannot index object of type canvas");
        }

        public override string ToString()
        {
            return Number.ToString();
        }
    }
}
