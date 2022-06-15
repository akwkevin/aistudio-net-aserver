using Coldairarrow.Business.OA_Manage;
using Coldairarrow.Entity.OA_Manage;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Services.DefinitionStorage;

namespace AIStudio.Service.WorkflowCore
{
    /// <summary>
    /// 工作流
    /// </summary>
    public static class WorkflowExtension
    {
        public static List<OA_DefForm> DefForms = new List<OA_DefForm>();
        /// <summary>
        /// 初始化工作流
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWorkflow(this IApplicationBuilder app)
        {
            IServiceProvider services = app.ApplicationServices;

            IDefinitionLoader _definitionLoader = services.GetService<IDefinitionLoader>();     
            ILogger _logger = services.GetService<ILogger<OAExtension>>();

      
            foreach(var defform in DefForms)
            {
                try
                {
                    var def = _definitionLoader.LoadDefinition(defform.WorkflowJSON, Deserializers.Json);
                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Debug, new EventId((int)UserLogType.工作流程, UserLogType.工作流程.ToString()), "工作流" +def.Id + "-" + def.Version + "加载成功");
                }
                catch(Exception ex)
                {
                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, new EventId((int)UserLogType.工作流程, UserLogType.工作流程.ToString()), "工作流" + defform.Name +"-" + ex.Message);
                }
            }

            var host = app.ApplicationServices.GetService<IWorkflowHost>();
            host.Start();

            return app;
        }

       
    }
}
