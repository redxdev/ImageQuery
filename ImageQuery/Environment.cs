using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageQuery.Canvas;

namespace ImageQuery
{
    public class Environment
    {
        private Dictionary<string, ICanvas> _canvases = new Dictionary<string, ICanvas>();
        private List<ICanvas> _inputs = new List<ICanvas>();
        private List<ICanvas> _outputs = new List<ICanvas>();

        private ICanvasLoader _canvasLoader = null;

        public void SetCanvasLoader(ICanvasLoader loader)
        {
            _canvasLoader = loader;
        }

        void CreateInput(string name)
        {
            if (_canvasLoader == null)
                throw new InvalidOperationException("Cannot create new input canvas when the canvas loader is null");

            if (name == null)
                throw new ArgumentNullException("name", "Cannot use a null name for a canvas");

            ICanvas canvas = _canvasLoader.LoadCanvas(name);
            if (canvas == null)
                throw new ArgumentException(string.Format("Received null canvas for {0}", name), "name");

            if (_canvases.ContainsKey(canvas.Name))
                throw new ArgumentException(string.Format("A canvas with the name {0} already exists", canvas.Name), "name");

            _canvases.Add(canvas.Name, canvas);
            _inputs.Add(canvas);
        }

        void CreateOutput(string name, uint w, uint h)
        {
            if (name == null)
                throw new ArgumentNullException("name", "Cannot use a null name for a canvas");

            ICanvas canvas = new BasicCanvas(CanvasMode.WriteOnly, name, w, h);
            if(_canvases.ContainsKey(canvas.Name))
                throw new ArgumentException(string.Format("A canvas with the name {0} already exists", canvas.Name), name);

            _canvases.Add(canvas.Name, canvas);
            _outputs.Add(canvas);
        }

        void CreateIntermediate(string name, uint w, uint h)
        {
            if (name == null)
                throw new ArgumentNullException("name", "Cannot use a null name for a canvas");

            ICanvas canvas = new BasicCanvas(CanvasMode.WriteOnly, name, w, h);
            if (_canvases.ContainsKey(canvas.Name))
                throw new ArgumentException(string.Format("A canvas with the name {0} already exists", canvas.Name), name);

            _canvases.Add(canvas.Name, canvas);
        }
    }
}
