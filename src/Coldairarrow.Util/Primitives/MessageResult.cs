using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coldairarrow.Util
{
    //aistudio
    public class MessageResult : AjaxResult
    {
        /// <summary>
        /// 类型
        /// </summary>
        public WSMessageType MessageType { get; set; }

        public object Data { get; set; }

    }
}
