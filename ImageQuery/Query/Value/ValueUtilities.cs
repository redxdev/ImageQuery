using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageQuery.Query.Value
{
    public static class ValueUtilities
    {
        public static void SetValue(this IQueryValue l, IQueryValue r)
        {
            if (l.GetIQLType() != r.GetIQLType())
            {
                throw new InvalidOperationException("Cannot set query value to disparate type");
            }

            switch (l.GetIQLType())
            {
                case IQLType.Number:
                    l.Number = r.Number;
                    break;

                case IQLType.Boolean:
                    l.Boolean = r.Boolean;
                    break;

                case IQLType.Canvas:
                    l.Canvas = r.Canvas;
                    break;

                case IQLType.Color:
                    l.Color = r.Color.Clone();
                    break;
            }
        }
    }
}
