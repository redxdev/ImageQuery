using System;

namespace ImageQuery.Library
{
    public static class FunctionUtils
    {
        public static void ThrowArgumentCountException(string func, string expected, int given)
        {
            throw new InvalidOperationException(string.Format("\"{0}\" expects {1} arguments ({2} given)", func, expected, given));
        }

        public static void CheckArgumentCountEqual(string func, int expected, int given)
        {
            if (expected != given)
                ThrowArgumentCountException(func, expected.ToString(), given);
        }

        public static void CheckArgumentCountBetween(string func, int low, int high, int given)
        {
            if(given < low || given > high)
                ThrowArgumentCountException(func, low + "-" + high, given);
        }
    }
}
