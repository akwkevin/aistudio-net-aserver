using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coldairarrow.Util
{
    public static class DistributedCacheExtensions
    {
        public static void Test(this IDistributedCache cache)
        {
            if (cache is MemoryDistributedCache memory)
            {
                 
            }
        }
    }
}
