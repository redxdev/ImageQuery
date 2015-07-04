using System;
using System.Collections.Generic;
using ImageQuery.Canvas;
using ImageQuery.Environment.Attributes;
using ImageQuery.Query.Value;

namespace ImageQuery.Environment
{
    public class RootEnvironment : IEnvironment
    {
        public RootEnvironment(IQLSettings settings)
        {
            Settings = settings;
        }

        public IQLSettings Settings { get; private set; }

        private List<ICanvas> _inputs = new List<ICanvas>();
        private List<ICanvas> _outputs = new List<ICanvas>();

        private ICanvasLoader _canvasLoader = null;

        private Dictionary<string, IQueryValue> _variables = new Dictionary<string, IQueryValue>();
        private Dictionary<string, IQueryValue> _parameters = new Dictionary<string, IQueryValue>();
        private Dictionary<string, QueryFunction> _functions = new Dictionary<string, QueryFunction>(); 

        public void SetCanvasLoader(ICanvasLoader loader)
        {
            _canvasLoader = loader;
        }

        public ICanvas CreateInput(string name)
        {
            if (_canvasLoader == null)
                throw new InvalidOperationException("Cannot create new input canvas when the canvas loader is null");

            if (name == null)
                throw new ArgumentNullException("name", "Cannot use a null name for a canvas");

            ICanvas canvas = _canvasLoader.LoadCanvas(name);
            if (canvas == null)
                throw new ArgumentException(string.Format("Received null canvas for \"{0}\"", name), "name");

            CanvasValue value = new CanvasValue() {Canvas = canvas};
            CreateVariable(canvas.Name, value);
            _inputs.Add(canvas);

            return canvas;
        }

        public ICanvas CreateOutput(string name, int w, int h)
        {
            if (name == null)
                throw new ArgumentNullException("name", "Cannot use a null name for a canvas");

            ICanvas canvas = new BasicCanvas(CanvasMode.WriteOnly, name, w, h);

            CreateVariable(canvas.Name, new CanvasValue() {Canvas = canvas});

            _outputs.Add(canvas);

            return canvas;
        }

        public ICanvas CreateIntermediate(string name, int w, int h)
        {
            if (name == null)
                throw new ArgumentNullException("name", "Cannot use a null name for a canvas");

            ICanvas canvas = new BasicCanvas(CanvasMode.WriteOnly, name, w, h);
            CreateVariable(canvas.Name, new CanvasValue() {Canvas = canvas});

            return canvas;
        }

        public ICanvas[] GetOutputs()
        {
            return _outputs.ToArray();
        }

        public void CreateVariable(string name, IQueryValue value)
        {
            if (_variables.ContainsKey(name))
                throw new ArgumentException(string.Format("A variable with the name \"{0}\" already exists", name), "name");

            _variables.Add(name, value);
        }

        public void SetVariable(string name, IQueryValue value)
        {
            _variables[name] = value;
        }

        public IQueryValue GetVariable(string name)
        {
            IQueryValue value = null;
            if (!_variables.TryGetValue(name, out value))
                throw new KeyNotFoundException(string.Format("Variable \"{0}\" is not defined", name));

            return value;
        }

        public void CreateParameter(string name, IQueryValue value)
        {
            _parameters.Add(name, value);
        }

        public IQueryValue GetParameter(string name)
        {
            IQueryValue value = null;
            if (!_parameters.TryGetValue(name, out value))
                throw new KeyNotFoundException(string.Format("Parameter \"{0}\" is not defined", name));

            return value;
        }

        public bool HasParameter(string name)
        {
            return _parameters.ContainsKey(name);
        }

        public void RegisterFunction(string name, QueryFunction func)
        {
            _functions.Add(name, func);
        }

        public QueryFunction GetFunction(string name)
        {
            QueryFunction func = null;
            if(!_functions.TryGetValue(name, out func))
                throw new KeyNotFoundException(string.Format("Function \"{0}\" is not defined", name));

            return func;
        }

        public void RegisterFunctions()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.DefinedTypes)
                {
                    foreach (var method in type.DeclaredMethods)
                    {
                        foreach (
                            var attr in
                                (QueryFunctionAttribute[])
                                    Attribute.GetCustomAttributes(method, typeof (QueryFunctionAttribute)))
                        {
                            if(!method.IsStatic)
                                throw new InvalidOperationException(String.Format("Method \"{0}.{1}\" must be static for QueryFunctionAttribute use", type.FullName, type.FullName));

                            if(method.IsAbstract)
                                throw new InvalidOperationException(String.Format("Method \"{0}.{1}\" cannot be abstract for QueryFunctionAttribute use", type.FullName, method.Name));

                            RegisterFunction(attr.Name, (QueryFunction)method.CreateDelegate(typeof(QueryFunction)));
                        }
                    }
                }
            }
        }
    }
}
