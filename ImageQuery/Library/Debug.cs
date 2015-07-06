using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Environment;
using ImageQuery.Environment.Attributes;
using ImageQuery.Query.Value;

namespace ImageQuery.Library
{
    public static class Debug
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
    }
}
