using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Canvas;

namespace ImageQuery.Query.Value
{
    public class NullValue : IQueryValue
    {
        public float Number
        {
            get
            {
                throw new NullReferenceException("Cannot access NullValue type");
            }

            set
            {
                throw new NullReferenceException("Cannot access NullValue type");
            }
        }

        public Color Color
        {
            get
            {
                throw new NullReferenceException("Cannot access NullValue type");
            }

            set
            {
                throw new NullReferenceException("Cannot access NullValue type");
            }
        }

        public ICanvas Canvas
        {
            get
            {
                throw new NullReferenceException("Cannot access NullValue type");
            }

            set
            {
                throw new NullReferenceException("Cannot access NullValue type");
            }
        }

        public bool Boolean
        {
            get
            {
                throw new NullReferenceException("Cannot access NullValue type");
            }

            set
            {
                throw new NullReferenceException("Cannot access NullValue type");
            }
        }

        public IQLType GetIQLType()
        {
            return IQLType.None;
        }

        public IQueryValue Index(IQueryValue x, IQueryValue y)
        {
            throw new InvalidOperationException("Cannot index object of type null");
        }

        public override string ToString()
        {
            return "null";
        }
    }
}
