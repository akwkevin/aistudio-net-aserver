using Coldairarrow.Business.Quartz_Manage;
using Coldairarrow.Util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AIStudio.Service.Quartz
{
    public class FileQuartz
    {
        private static IQuartz_TaskBusiness _quartz_TaskBusiness { get => ServiceLocator.Instance.GetRequiredService<IQuartz_TaskBusiness>(); }
        private static ILogger _logger { get => ServiceLocator.Instance.GetService<ILogger<FileQuartz>>(); }


        public static async Task SaveJob(List<Quartz_TaskDTO> taskList)
        {
            foreach (var task in taskList)
            {
                if (task.CreateTime == DateTime.MinValue)
                {
                    task.CreateTime = DateTime.Now;
                    await _quartz_TaskBusiness.InsertAsync(task);
                }
                else
                {
                    await _quartz_TaskBusiness.UpdateAsync(task);
                }
            }
        }

        public static void WriteDebug(string content)
        {
            _logger.LogDebug(UserLogType.系统任务.ToEventId(), content);
        }

        public static void WriteLog(string content)
        {
            _logger.LogInformation(UserLogType.系统任务.ToEventId(), content);
        }
        public static void WriteJobAction(JobAction jobAction, ITrigger trigger, string taskName, string groupName)
        {
            WriteJobAction(jobAction, taskName, groupName, trigger == null ? "未找到作业" : "OK");
        }
        public static void WriteJobAction(JobAction jobAction, string taskName, string groupName, string content = null)
        {
            content = $"{jobAction.ToString()} --分组：{groupName},作业：{taskName},消息:{content ?? "OK"}";
            _logger.LogInformation(UserLogType.系统任务.ToEventId(), content);
        }

        public static void WriteJobExecute(LogLevel logLevel, string jobFullName, string content)
        {
            _logger.Log(logLevel, UserLogType.系统任务执行.ToEventId(), content, jobFullName);
        }
    }
}
