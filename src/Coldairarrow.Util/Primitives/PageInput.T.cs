using System.Collections.Generic;

namespace Coldairarrow.Util
{
    public class PageInput<T> : PageInput where T : new()
    {
        public T Search { get; set; } = new T();

        public Dictionary<string, object> SearchKeyValues { get; set; } = new Dictionary<string, object>();
    }


}
