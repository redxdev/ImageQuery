using System;
using ImageQuery.Environment;
using ImageQuery.Environment.Attributes;
using ImageQuery.Query.Value;

namespace ImageQuery.Library
{
    public static class ImageMath
    {
        [QueryFunction("sin")]
        public static IQueryValue Sin(IEnvironment env, IQueryValue[] args)
        {
            FunctionUtils.CheckArgumentCountEqual("sin", 1, args.Length);

            return new NumberValue() {Number = (float) Math.Sin(args[0].Number)};
        }

        [QueryFunction("csin")]
        public static IQueryValue CSin(IEnvironment env, IQueryValue[] args)
        {
            FunctionUtils.CheckArgumentCountBetween("csin", 1, 2, args.Length);

            return new ColorValue()
            {
                Color = args[0].Color.Each(v => (float) Math.Sin(v), args.Length == 2 && args[1].Boolean)
            };
        }

        [QueryFunction("cos")]
        public static IQueryValue Cos(IEnvironment env, IQueryValue[] args)
        {
            FunctionUtils.CheckArgumentCountEqual("cos", 1, args.Length);

            return new NumberValue() { Number = (float)Math.Cos(args[0].Number) };
        }

        [QueryFunction("ccos")]
        public static IQueryValue CCos(IEnvironment env, IQueryValue[] args)
        {
            FunctionUtils.CheckArgumentCountBetween("ccos", 1, 2, args.Length);

            return new ColorValue()
            {
                Color = args[0].Color.Each(v => (float)Math.Cos(v), args.Length == 2 && args[1].Boolean)
            };
        }
    }
}
