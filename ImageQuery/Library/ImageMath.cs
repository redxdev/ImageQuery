using System;
using ImageQuery.Canvas;
using ImageQuery.Environment;
using ImageQuery.Environment.Attributes;
using ImageQuery.Query.Value;

namespace ImageQuery.Library
{
    public static class ImageMath
    {
        [QueryFunction("abs")]
        public static IQueryValue Abs(IEnvironment env, IQueryValue[] args)
        {
            FunctionUtils.CheckArgumentCountEqual("abs", 1, args.Length);

            return new NumberValue() {Number = Math.Abs(args[0].Number)};
        }

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

        [QueryFunction("tan")]
        public static IQueryValue Tan(IEnvironment env, IQueryValue[] args)
        {
            FunctionUtils.CheckArgumentCountEqual("tan", 1, args.Length);

            return new NumberValue() {Number = (float) Math.Tan(args[0].Number)};
        }

        [QueryFunction("ctan")]
        public static IQueryValue CTan(IEnvironment env, IQueryValue[] args)
        {
            FunctionUtils.CheckArgumentCountBetween("ctan", 1, 2, args.Length);

            return new ColorValue()
            {
                Color = args[0].Color.Each(v => (float) Math.Tan(v), args.Length == 2 && args[1].Boolean)
            };
        }

        [QueryFunction("magnitude")]
        public static IQueryValue Magnitude(IEnvironment env, IQueryValue[] args)
        {
            FunctionUtils.CheckArgumentCountBetween("magnitude", 1, 2, args.Length);

            Color color = args[0].Color;
            float result = color.R*color.R + color.G*color.G + color.B*color.B;
            if (args.Length == 2 && args[1].Boolean == true)
                result += color.A*color.A;
            result = (float) Math.Sqrt(result);

            return new NumberValue() {Number = result};
        }

        [QueryFunction("clamp")]
        public static IQueryValue Clamp(IEnvironment env, IQueryValue[] args)
        {
            FunctionUtils.CheckArgumentCountEqual("clamp", 3, args.Length);

            return new NumberValue() {Number = args[0].Number.Clamp(args[1].Number, args[2].Number)};
        }

        [QueryFunction("cclamp")]
        public static IQueryValue CClamp(IEnvironment env, IQueryValue[] args)
        {
            FunctionUtils.CheckArgumentCountEqual("cclamp", 1, args.Length);
            Color color = args[0].Color;
            color.Clamp();
            return new ColorValue() {Color = color};
        }
    }
}
