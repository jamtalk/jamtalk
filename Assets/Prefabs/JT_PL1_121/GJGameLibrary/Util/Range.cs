using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJGameLibrary.Util
{
    public class Range<T>
    {
        public T Min { get; set; }
        public T Max { get; set; }
        public Range(T Min, T Max)
        {
            this.Min = Min;
            this.Max = Max;
        }
    }
}
