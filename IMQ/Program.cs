using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ImageQuery.Canvas;
using ImageQuery.Environment;
using ImageQuery.Language;
using ImageQuery.Query.Statements;

namespace IMQ
{
    class Program
    {
        private static void PrintUsage()
        {
            Console.WriteLine("IQM [-h|--help] [-n|--no-parallel] [-p|--process-count number] [-i|--input name path] [-o|--output name path] <script>");
            Console.WriteLine("Options:");
            Console.WriteLine("  -h  --help           Display this help text.");
            Console.WriteLine("  -n  --no-parallel    Disable parallel processing (default: parallel processing enabled).");
            Console.WriteLine("  -p  --process-count  Set the number of processes used for parallel processing (default: # of logical processors).");
            Console.WriteLine("  -i  --input          Define the location for an input.");
            Console.WriteLine("        script         The path to the IQL script to execute.");
        }

        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                PrintUsage();
                return;
            }

            IQLSettings settings = new IQLSettings();
            settings.AllowParallel = true;
            settings.ProcessCount = Environment.ProcessorCount;

            BitmapCanvasLoader canvasLoader = new BitmapCanvasLoader();
            Dictionary<string, string> outputPaths = new Dictionary<string, string>();
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

                    case "--no-parallel":
                    case "-n":
                        settings.AllowParallel = false;
                        break;

                    case "--process-count":
                    case "-p":
                    {
                        ++i;
                        if (i >= args.Length)
                        {
                            Console.WriteLine("{0} requires 1 argument", arg);
                            return;
                        }

                        int value;
                        if (!int.TryParse(args[i], out value) || value < 1)
                        {
                            Console.WriteLine("Argument to {0} must be an integer >= 1", arg);
                            return;
                        }

                        settings.ProcessCount = value;
                        break;
                    }

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

            if (string.IsNullOrEmpty(scriptFilename))
            {
                Console.WriteLine("No input script was specified.");
                return;
            }

            if (outputPaths.Count == 0)
            {
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("WARNING: No output paths were specified.");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }

            if (settings.AllowParallel)
            {
                Console.WriteLine("Running in parallel mode with {0} threads", settings.ProcessCount);
            }
            else
            {
                Console.WriteLine("Running in single-threaded mode");
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("Loading IQL...");

            RootEnvironment root = new RootEnvironment(settings);
            root.SetCanvasLoader(canvasLoader);

            IQueryStatement[] statements = null;

            try
            {
                statements = LanguageUtilities.ParseFile(scriptFilename);
            }
            catch (IOException e)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Unable to open script file:");
                Console.WriteLine(e);
                Console.BackgroundColor = ConsoleColor.Black;
                return;
            }
            catch (ParseException e)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Unable to parse IQL:");
                Console.WriteLine(e);
                Console.BackgroundColor = ConsoleColor.Black;
                return;
            }

            double parseTime = stopwatch.Elapsed.TotalSeconds;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Parse completed in {0} seconds", parseTime);
            Console.ForegroundColor = ConsoleColor.White;
            stopwatch.Restart();

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
                return;
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
                }
            }

            double writeTime = stopwatch.Elapsed.TotalSeconds;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Write completed in {0} seconds", writeTime);
            Console.ForegroundColor = ConsoleColor.White;
            stopwatch.Stop();

            Console.WriteLine("Total time: {0} seconds", parseTime + queryTime + writeTime);
        }
    }
}
