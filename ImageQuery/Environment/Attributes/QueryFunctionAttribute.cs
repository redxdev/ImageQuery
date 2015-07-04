using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageQuery.Environment.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class QueryFunctionAttribute : Attribute
    {
        public QueryFunctionAttribute(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }

        private string _name;
    }
}
