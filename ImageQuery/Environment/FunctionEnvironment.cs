using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Canvas;
using ImageQuery.Query.Value;

namespace ImageQuery.Environment
{
    public class FunctionEnvironment : IEnvironment
    {
        public FunctionEnvironment(IEnvironment env)
        {
            Parent = env;
        }

        public IQLSettings Settings { get { return Parent.Settings; } }

        public IEnvironment Parent { get; private set; }

        private Dictionary<string, IQueryValue> _variables = new Dictionary<string, IQueryValue>();

        public ICanvas CreateInput(string name)
        {
            throw new InvalidOperationException("Cannot create an input inside this context");
        }

        public ICanvas CreateOutput(string name, int w, int h)
        {
            throw new InvalidOperationException("Cannot create an output inside this context");
        }

        public ICanvas CreateIntermediate(string name, int w, int h)
        {
            if (name == null)
                throw new ArgumentNullException("name", "Cannot use a null name for a canvas");

            ICanvas canvas = new BasicCanvas(CanvasMode.ReadWrite, name, w, h);
            CreateVariable(canvas.Name, new CanvasValue() {Canvas = canvas});

            return canvas;
        }

        public void CreateVariable(string name, IQueryValue value)
        {
            if (_variables.ContainsKey(name))
                throw new ArgumentException(string.Format("A variable with the name \"{0}\" already exists", name), "name");

            _variables.Add(name, value);
        }

        public void SetVariable(string name, IQueryValue value)
        {
            if(_variables.ContainsKey(name))
                _variables[name] = value;
            else
                Parent.SetVariable(name, value);
        }

        public IQueryValue GetVariable(string name)
        {
            if (_variables.ContainsKey(name))
                return _variables[name];
            else
                return Parent.GetVariable(name);
        }

        public void CreateParameter(string name, IQueryValue value)
        {
            throw new InvalidOperationException("Cannot create a parameter inside this context");
        }

        public IQueryValue GetParameter(string name)
        {
            throw new InvalidOperationException("Cannot retreive a parameter inside this context");
        }

        public bool HasParameter(string name)
        {
            throw new InvalidOperationException("Cannot check for a parameter inside this context");
        }

        public void RegisterFunction(string name, QueryFunction func)
        {
            throw new InvalidOperationException("Cannot register a function inside this context");
        }

        public QueryFunction GetFunction(string name)
        {
            return Parent.GetFunction(name);
        }
    }
}
