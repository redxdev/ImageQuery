using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery;
using ImageQuery.Canvas;
using ImageQuery.Query;
using ImageQuery.Query.Operators;

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

            try
            {
                BitmapCanvasLoader loader = new BitmapCanvasLoader();
                loader.RegisterName("in", "in.png");
                root.SetCanvasLoader(loader);

                ICanvas input = root.CreateInput("in");
                ICanvas output = root.CreateOutput("out", input.Width, input.Height);
                ApplyStatement apply = new ApplyStatement()
                {
                    CanvasName = "out",
                    Selection = new BasicSelection()
                    {
                        CanvasName = "in",
                        Modulation = new AddExpression()
                        {
                            Left = new SubtractExpression()
                            {
                                Left = new ColorExpression()
                                {
                                    R = new NumberExpression()
                                    {
                                        Value = 1
                                    },
                                    A = new NumberExpression()
                                    {
                                        Value = 0
                                    }
                                },
                                Right = new RetrieveVariableExpression()
                                {
                                    Name = "color"
                                }
                            },
                            Right = new ColorExpression()
                            {
                                A = new NumberExpression()
                                {
                                    Value = 1
                                }
                            }
                        },
                        Where = new EqualityExpression()
                        {
                            Left = new ModulusExpression()
                            {
                                Left = new RetrieveVariableExpression()
                                {
                                    Name = "x"
                                },
                                Right = new NumberExpression()
                                {
                                    Value = 2
                                }
                            },
                            Right = new NumberExpression()
                            {
                                Value = 0
                            }
                        }
                    }
                };

                apply.Run(root);

                output.WriteToFile("out.png");
                Console.WriteLine("Wrote output to out.png");
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Exception encountered while running query:");
                Console.WriteLine(e);
                Console.BackgroundColor = ConsoleColor.Black;
            }

            Console.ReadKey();
        }
    }
}
