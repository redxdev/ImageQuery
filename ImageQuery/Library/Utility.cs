using System;
using System.Linq;
using ImageQuery.Environment;
using ImageQuery.Environment.Attributes;
using ImageQuery.Query.Value;

namespace ImageQuery.Library
{
    public static class Utility
    {
        [QueryFunction("print")]
        public static IQueryValue Print(IEnvironment env, IQueryValue[] args)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            if (args.Length == 0)
            {
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine(string.Join(" ", args.Select(v => v.ToString()).ToArray()));
            }

            Console.ForegroundColor = ConsoleColor.White;

            return new NullValue();
        }

        [QueryFunction("par")]
        public static IQueryValue PrintAndReturn(IEnvironment env, IQueryValue[] args)
        {
            FunctionUtils.CheckArgumentCountEqual("par", 1, args.Length);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(args[0].ToString());
            Console.ForegroundColor = ConsoleColor.White;

            return args[0];
        }

        [QueryFunction("null")]
        public static IQueryValue Null(IEnvironment env, IQueryValue[] args)
        {
            return new NullValue();
        }
    }
}
