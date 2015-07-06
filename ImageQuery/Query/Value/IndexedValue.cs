using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Canvas;

namespace ImageQuery.Query.Value
{
    public class IndexedValue : IQueryValue
    {
        public IQueryValue BaseValue { get; set; }

        public Func<IQueryValue, IQueryValue, IQueryValue> IndexOperation { get; set; } 

        public float Number
        {
            get
            {
                if (BaseValue == null)
                {
                    throw new InvalidOperationException("Cannot retrieve from index-only value");
                }

                return BaseValue.Number;
            }

            set
            {
                if (BaseValue == null)
                {
                    throw new InvalidOperationException("Cannot set index-only value");
                }

                BaseValue.Number = value;
            }
        }

        public Color Color
        {
            get
            {
                if (BaseValue == null)
                {
                    throw new InvalidOperationException("Cannot retrieve from index-only value");
                }

                return BaseValue.Color;
            }

            set
            {
                if (BaseValue == null)
                {
                    throw new InvalidOperationException("Cannot set index-only value");
                }

                BaseValue.Color = value;
            }
        }

        public ICanvas Canvas
        {
            get
            {
                if (BaseValue == null)
                {
                    throw new InvalidOperationException("Cannot retrieve from index-only value");
                }

                return BaseValue.Canvas;
            }

            set
            {
                if (BaseValue == null)
                {
                    throw new InvalidOperationException("Cannot set index-only value");
                }

                BaseValue.Canvas = value;
            }
        }

        public bool Boolean
        {
            get
            {
                if (BaseValue == null)
                {
                    throw new InvalidOperationException("Cannot retrieve from index-only value");
                }

                return BaseValue.Boolean;
            }

            set
            {
                if (BaseValue == null)
                {
                    throw new InvalidOperationException("Cannot set index-only value");
                }

                BaseValue.Boolean = value;
            }
        }

        public IQLType GetIQLType()
        {
            if (BaseValue == null)
                throw new InvalidOperationException("Index-only values have no type");

            return BaseValue.GetIQLType();
        }

        public IQueryValue Index(IQueryValue x, IQueryValue y)
        {
            if(IndexOperation == null)
                throw new InvalidOperationException("IndexOperation is not set for IndexedValue");

            return IndexOperation(x, y);
        }

        public override string ToString()
        {
            return string.Format("Indexed{{{0}}}", BaseValue == null ? "null" : BaseValue.ToString());
        }
    }
}
