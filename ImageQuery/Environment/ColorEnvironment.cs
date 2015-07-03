using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Canvas;
using ImageQuery.Query.Value;

namespace ImageQuery.Environment
{
    public class ColorEnvironment : IEnvironment
    {
        public ColorEnvironment(IEnvironment env, Color color)
        {
            Parent = env;
            Color = color;
        }

        public IQLSettings Settings
        {
            get { return Parent.Settings; }
        }

        public IEnvironment Parent { get; private set; }

        public Color Color { get; private set; }

        public ICanvas CreateInput(string name)
        {
            throw new InvalidOperationException("Cannot create a variable inside this context");
        }

        public ICanvas CreateOutput(string name, int w, int h)
        {
            throw new InvalidOperationException("Cannot create a variable inside this context");
        }

        public ICanvas CreateIntermediate(string name, int w, int h)
        {
            throw new InvalidOperationException("Cannot create a variable inside this context");
        }

        public void CreateVariable(string name, IQueryValue value)
        {
            throw new InvalidOperationException("Cannot create a variable inside this context");
        }

        public virtual void SetVariable(string name, IQueryValue value)
        {
            switch (name)
            {
                case "r":
                case "red":
                case "g":
                case "green":
                case "b":
                case "blue":
                case "a":
                case "alpha":
                    throw new ArgumentException(string.Format("Cannot set {0} as it is protected in this context", name), "name");

                default:
                    throw new KeyNotFoundException(string.Format("Variable {0} does not exist in this context", name));
            }
        }

        public virtual IQueryValue GetVariable(string name)
        {
            switch (name)
            {
                case "red":
                case "r":
                    return new NumberValue() {Number = Color.R};

                case "green":
                case "g":
                    return new NumberValue() {Number = Color.G};

                case "blue":
                case "b":
                    return new NumberValue() {Number = Color.B};

                case "alpha":
                case "a":
                    return new NumberValue() {Number = Color.A};

                default:
                    throw new KeyNotFoundException(string.Format("Variable {0} does not exist in this context", name));
            }
        }
    }
}
