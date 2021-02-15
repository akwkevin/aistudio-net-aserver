using Coldairarrow.Business.OA_Manage;
using Coldairarrow.Entity.OA_Manage;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Service.WorkflowCore
{
    /// <summary>
    /// 中间节点
    /// </summary>
    public class OAMiddleStep : OABaseStep
    {
        /// <summary>
        /// 
        /// </summary>
        public OAMiddleStep()
        {

        }


        /// <summary>
        /// 节点触发
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            return await base.RunAsync(context);
        }
    }
}
