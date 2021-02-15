using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIStudio.Service.Quartz
{
    /// <summary>
    /// 
    /// </summary>
    public enum JobAction
    {
        /// <summary>
        /// 
        /// </summary>
        新增 = 1,
        /// <summary>
        /// 
        /// </summary>
        删除 = 2,
        /// <summary>
        /// 
        /// </summary>
        修改 = 3,
        /// <summary>
        /// 
        /// </summary>
        暂停 = 4,
        /// <summary>
        /// 
        /// </summary>
        停止,
        /// <summary>
        /// 
        /// </summary>
        开启,
        /// <summary>
        /// 
        /// </summary>
        立即执行
    }
}
