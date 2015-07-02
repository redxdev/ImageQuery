using System;
using System.Collections.Generic;
using ImageQuery.Canvas;
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
                throw new ArgumentException(string.Format("Received null canvas for {0}", name), "name");

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

        ICanvas[] GetOutputs()
        {
            return _outputs.ToArray();
        }

        public void CreateVariable(string name, IQueryValue value)
        {
            if (_variables.ContainsKey(name))
                throw new ArgumentException(string.Format("A variable with the name {0} already exists", name), "name");

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
                throw new KeyNotFoundException(string.Format("Unknown variable {0}", name));

            return value;
        }
    }
}
