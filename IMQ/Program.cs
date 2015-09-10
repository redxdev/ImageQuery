using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ImageQuery.Canvas;
using ImageQuery.Environment;
using ImageQuery.Language;
using ImageQuery.Query.Statements;
using ImageQuery.Query.Value;

namespace IMQ
{
    class Program
    {
        private static void PrintUsage()
        {
            Console.WriteLine("IMQ [options] [filename]");
            Console.WriteLine("Options:");
            Console.WriteLine("  -c  --interactive             Start in interactive console mode. Inputs and outputs will be automatically defined and saved.");
            Console.WriteLine("  -d  --define <name> <value>   Define a parameter for the script.");
            Console.WriteLine("  -h  --help                    Display this help text.");
            Console.WriteLine("  -i  --input  <name> <path>    Define the location for an input.");
            Console.WriteLine("  -o  --output <name> <path>    Define the location for an output.");
            Console.WriteLine("  -p  --parallel                Enable parallel processing/threading (default: no threading).");
            Console.WriteLine("        [filename]              The path to the IQL script to execute (ignored in interactive mode).");
        }

        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                PrintUsage();
                return;
            }

            IQLSettings settings = new IQLSettings();
            settings.AllowParallel = false;

            BitmapCanvasLoader canvasLoader = new BitmapCanvasLoader();
            LinkedList<string> inputNames = new LinkedList<string>();
            Dictionary<string, string> outputPaths = new Dictionary<string, string>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            bool interactive = false;
            string scriptFilename = string.Empty;

            int i = 0;
            while (i < args.Length)
            {
                string arg = args[i];
                switch (arg)
                {
                    case "--help":
                    case "-h":
                        PrintUsage();
                        return;

                    case "--parallel":
                    case "-p":
                        settings.AllowParallel = true;
                        break;

                    case "--input":
                    case "-i":
                    {
                        ++i;
                        if (i >= args.Length)
                        {
                            Console.WriteLine("{0} requires 2 arguments", arg);
                            return;
                        }
                        string key = args[i];

                        ++i;
                        if (i >= args.Length)
                        {
                            Console.WriteLine("{0} requires 2 arguments", arg);
                            return;
                        }
                        string value = args[i];

                        canvasLoader.RegisterName(key, value);
                        inputNames.AddLast(key);
                        break;
                    }

                    case "--output":
                    case "-o":
                    {
                        ++i;
                        if (i >= args.Length)
                        {
                            Console.WriteLine("{0} requires 2 arguments", arg);
                            return;
                        }
                        string key = args[i];

                        ++i;
                        if (i >= args.Length)
                        {
                            Console.WriteLine("{0} requires 2 arguments", arg);
                            return;
                        }
                        string value = args[i];

                        outputPaths.Add(key, value);
                        break;
                    }

                    case "-d":
                    case "--define":
                    {
                        ++i;
                        if (i >= args.Length)
                        {
                            Console.WriteLine("{0} requires 2 arguments", arg);
                            return;
                        }
                        string name = args[i];

                        ++i;
                        if (i > args.Length)
                        {
                            Console.WriteLine("{0} requires 2 arguments", arg);
                            return;
                        }
                        string value = args[i];

                        parameters.Add(name, value);
                        break;
                    }

                    case "-c":
                    case "--interactive":
                        interactive = true;
                        break;

                    default:
                        if (!string.IsNullOrEmpty(scriptFilename))
                        {
                            Console.WriteLine("You may only specify a single script file at a time.");
                            return;
                        }

                        scriptFilename = arg;
                        break;
                }

                ++i;
            }

            if (!interactive && string.IsNullOrEmpty(scriptFilename))
            {
                Console.WriteLine("No input script was specified while running in non-interactive mode.");
                return;
            }

            if (interactive && settings.AllowParallel)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Interactive and parallel modes are not supported at the same time.");
                Console.BackgroundColor = ConsoleColor.Black;
                return;
            }

            RootEnvironment root = new RootEnvironment(settings);
            root.RegisterFunctions();

            foreach (var entry in parameters)
            {
                try
                {
                    IQueryValue value = LanguageUtilities.ParseValueFromString(root, entry.Value);
                    root.CreateParameter(entry.Key, value);
                }
                catch (ParseException e)
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Parameter {0} is not a valid value:", entry.Key);
                    Console.WriteLine(e);
                    Console.BackgroundColor = ConsoleColor.Black;
                    return;
                }
                catch (Exception e)
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Unable to define parameter {0}:", entry.Key);
                    Console.WriteLine(e);
                    Console.BackgroundColor = ConsoleColor.Black;
                    return;
                }
            }

            if (outputPaths.Count == 0)
            {
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("WARNING: No output paths were specified.");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (interactive)
            {
                Console.WriteLine("Heads up! You've defined some output locations, but you'll have to create them in IQL to define their size.");
                Console.WriteLine("For example: output {0}[100,100]", outputPaths.Keys.First());
            }

            if (settings.AllowParallel)
            {
                Console.WriteLine("Running in parallel mode");
            }
            else
            {
                Console.WriteLine("Running in single-threaded mode");
            }

            root.SetCanvasLoader(canvasLoader);

            if (interactive)
            {
                foreach (var name in inputNames)
                {
                    try
                    {
                        var stm = new DefineInputStatement() { CanvasName = name };
                        stm.Run(root);
                    }
                    catch (Exception e)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Unable to register input \"{0}\":", name);
                        Console.WriteLine(e);
                        Console.BackgroundColor = ConsoleColor.Black;
                        return;
                    }
                }
            }

            do
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;

                IQueryStatement[] statements = null;

                try
                {
                    if (!interactive)
                        statements = LanguageUtilities.ParseFile(scriptFilename);
                    else
                    {
                        Console.Write("IQL> ");
                        statements = LanguageUtilities.ParseString(Console.ReadLine());
                    }
                }
                catch (IOException e)
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Unable to load script:");
                    Console.WriteLine(e);
                    Console.BackgroundColor = ConsoleColor.Black;
                    continue;
                }
                catch (ParseException e)
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Unable to parse IQL:");
                    Console.WriteLine(e);
                    Console.BackgroundColor = ConsoleColor.Black;
                    continue;
                }

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                try
                {
                    foreach (var statement in statements)
                    {
                        statement.Run(root);
                    }
                }
                catch (Exception e)
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Unable to execute IQL:");
                    Console.WriteLine(e);
                    Console.BackgroundColor = ConsoleColor.Black;
                    continue;
                }

                double queryTime = stopwatch.Elapsed.TotalSeconds;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Query completed in {0} seconds", queryTime);
                Console.ForegroundColor = ConsoleColor.White;
                stopwatch.Restart();

                ICanvas[] canvases = root.GetOutputs();

                foreach (var canvas in canvases)
                {
                    string outputFilename;
                    if (!outputPaths.TryGetValue(canvas.Name, out outputFilename))
                    {
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("Unknown output {0}, skipping", canvas.Name);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        continue;
                    }

                    Console.WriteLine("Writing output {0} to {1}...", canvas.Name, outputFilename);

                    try
                    {
                        canvas.WriteToFile(outputFilename);
                    }
                    catch (Exception e)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("Unable to write output {0} to file {1}:", canvas.Name, outputFilename);
                        Console.WriteLine(e);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }
                }

                double writeTime = stopwatch.Elapsed.TotalSeconds;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Write completed in {0} seconds", writeTime);
                Console.ForegroundColor = ConsoleColor.White;
                stopwatch.Stop();

                Console.WriteLine("Total time: {0} seconds", queryTime + writeTime);
            } while (interactive);
        }
    }
}
