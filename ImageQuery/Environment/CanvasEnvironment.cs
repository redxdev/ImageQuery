using System;
using System.Collections.Generic;
using ImageQuery.Canvas;
using ImageQuery.Query.Value;

namespace ImageQuery.Environment
{
    public class CanvasEnvironment : IEnvironment
    {
        public CanvasEnvironment(IEnvironment env, ICanvas canvas)
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

        public ICanvas CreateInput(string name)
        {
            throw new InvalidOperationException("Cannot create a variable inside a this context");
        }

        public ICanvas CreateOutput(string name, int w, int h)
        {
            throw new InvalidOperationException("Cannot create a variable inside a this context");
        }

        public ICanvas CreateIntermediate(string name, int w, int h)
        {
            throw new InvalidOperationException("Cannot create a variable inside a this context");
        }

        public void CreateVariable(string name, IQueryValue value)
        {
            throw new InvalidOperationException("Cannot create a variable inside a this context");
        }

        public virtual void SetVariable(string name, IQueryValue value)
        {
            switch (name)
            {
                case "width":
                case "height":
                    throw new ArgumentException(string.Format("Cannot set {0} as it is protected in this context", name), "name");

                default:
                    throw new KeyNotFoundException(string.Format("Variable {0} does not exist in this context", name));
                    break;
            }
        }

        public virtual IQueryValue GetVariable(string name)
        {
            switch (name)
            {
                case "width":
                    return new NumberValue() {Number = Canvas.Width};

                case "height":
                    return new NumberValue() {Number = Canvas.Height};

                default:
                    throw new KeyNotFoundException(string.Format("Variable {0} does not exist in this context", name));
            }
        }
    }
}