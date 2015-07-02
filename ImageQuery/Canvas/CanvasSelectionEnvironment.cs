using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageQuery.Canvas
{
    public class CanvasSelectionEnvironment : IEnvironment
    {
        public CanvasSelectionEnvironment(IEnvironment env, ICanvas canvas)
        {
            Parent = env;
            Canvas = canvas;
        }

        public IQLSettings Settings
        {
            get { return Parent.Settings; }
        }

        public IEnvironment Parent { get; private set; }

        public ICanvas Canvas { get; private set; }

        public int X { get; set; }
        public int Y { get; set; }

        public ICanvas CreateInput(string name)
        {
            return Parent.CreateInput(name);
        }

        public ICanvas CreateOutput(string name, int w, int h)
        {
            return Parent.CreateOutput(name, w, h);
        }

        public ICanvas CreateIntermediate(string name, int w, int h)
        {
            return Parent.CreateIntermediate(name, w, h);
        }

        public void CreateVariable(string name, IQueryValue value)
        {
            Parent.CreateVariable(name, value);
        }

        public void SetVariable(string name, IQueryValue value)
        {
            switch (name)
            {
                case "x":
                case "y":
                case "color":
                case "r":
                case "g":
                case "b":
                case "a":
                    throw new ArgumentException(string.Format("Cannot set {0} as it is protected in this context", name), "name");

                default:
                    Parent.SetVariable(name, value);
                    break;
            }
        }

        public IQueryValue GetVariable(string name)
        {
            switch (name)
            {
                case "x":
                    return new NumberValue() {Number = X};

                case "y":
                    return new NumberValue() {Number = Y};

                case "color":
                    return new ColorValue() {Color = Canvas[X, Y]};

                case "r":
                    return new NumberValue() {Number = Canvas[X, Y].R};

                case "g":
                    return new NumberValue() { Number = Canvas[X, Y].G };

                case "b":
                    return new NumberValue() { Number = Canvas[X, Y].B };

                case "a":
                    return new NumberValue() { Number = Canvas[X, Y].A };

                default:
                    return Parent.GetVariable(name);
            }
        }
    }
}
