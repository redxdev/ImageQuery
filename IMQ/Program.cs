using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery;
using ImageQuery.Canvas;
using ImageQuery.Environment;
using ImageQuery.Language;
using ImageQuery.Query;
using ImageQuery.Query.Operators;
using ImageQuery.Query.Statements;

namespace IMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Creating root environment...");
            IQLSettings settings = new IQLSettings()
            {
                AllowParallel = false,
                ProcessCount = 1
            };
            Console.WriteLine(settings);

            RootEnvironment root = new RootEnvironment(settings);

            string filename = args[0];

            try
            {
                BitmapCanvasLoader loader = new BitmapCanvasLoader();
                loader.RegisterName("in", "in.png");
                root.SetCanvasLoader(loader);

                IQueryStatement[] statements = LanguageUtilities.ParseFile(filename);
                foreach (IQueryStatement statement in statements)
                {
                    statement.Run(root);
                }

                root.GetVariable("out").Canvas.WriteToFile("out.png");
                Console.WriteLine("Wrote output to out.png");
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Exception encountered while running query:");
                Console.WriteLine(e);
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }
    }
}
