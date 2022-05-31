using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coldairarrow.Util
{
    public class Input<T> where T : new()
    {
        public T Search { get; set; } = new T();

        public Dictionary<string, object> SearchKeyValues { get; set; } = new Dictionary<string, object>();
    }
}
