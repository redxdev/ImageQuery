using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageQuery
{
    public struct IQLSettings
    {
        public bool AllowParallel { get; set; }
        public uint ProcessCount { get; set; }

        public override string ToString()
        {
            return string.Format("IQLSettings {{ AllowParallel = {0}, ProcessCount = {1} }}", AllowParallel, ProcessCount);
        }
    }
}
