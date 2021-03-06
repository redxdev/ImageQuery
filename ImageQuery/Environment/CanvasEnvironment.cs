﻿using System;
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
                case "width":
                case "height":
                    throw new ArgumentException(string.Format("Cannot set {0} as it is protected in this context", name), "name");

                default:
                    throw new KeyNotFoundException(string.Format("Variable {0} does not exist in this context", name));
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

        public void CreateParameter(string name, IQueryValue value)
        {
            Parent.CreateParameter(name, value);
        }

        public IQueryValue GetParameter(string name)
        {
            return Parent.GetParameter(name);
        }

        public bool HasParameter(string name)
        {
            return Parent.HasParameter(name);
        }

        public void RegisterFunction(string name, QueryFunction func)
        {
            Parent.RegisterFunction(name, func);
        }

        public QueryFunction GetFunction(string name)
        {
            return Parent.GetFunction(name);
        }
    }
}