using System;
using ImageQuery.Canvas;
using ImageQuery.Environment;
using ImageQuery.Environment.Attributes;
using ImageQuery.Query.Value;

namespace ImageQuery.Library
{
    public static class ImageRandom
    {
        private static Random random = new Random();

        [QueryFunction("rand")]
        public static IQueryValue Rand(IEnvironment env, IQueryValue[] args)
        {
            FunctionUtils.CheckArgumentCountEqual("rand", 0, args.Length);

            return new NumberValue() {Number = (float) random.NextDouble()};
        }

        [QueryFunction("crand")]
        public static IQueryValue CRand(IEnvironment env, IQueryValue[] args)
        {
            FunctionUtils.CheckArgumentCountBetween("rand", 0, 1, args.Length);

            return new ColorValue()
            {
                Color =
                    new Color((float) random.NextDouble(), (float) random.NextDouble(), (float) random.NextDouble(),
                        args.Length == 1 ? args[0].Number : (float) random.NextDouble())
            };
        }
    }
}
