using Coldairarrow.Business.D_Manage;
using Coldairarrow.Business.Quartz_Manage;
using Coldairarrow.Entity.D_Manage;
using Coldairarrow.Util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AIStudio.Service.Quartz
{
    public class SaveMessageJob : IJob
    {
        IQuene _quene { get { return ServiceLocator.Instance.GetRequiredService<IQuene>(); } }
        public async Task Execute(IJobExecutionContext context)
        {
            Quartz_TaskDTO taskOptions = context.GetTaskOptions();
            string message = "";
            AbstractTrigger trigger = (context as JobExecutionContextImpl).Trigger as AbstractTrigger;
            if (taskOptions == null)
            {
                FileQuartz.WriteJobExecute(LogLevel.Warning, trigger.FullName, "未到找作业或可能被移除");
            }

            LogLevel logLevel = LogLevel.Information;
            try
            {
                foreach (var type in GlobalData.BatchSaveTypes)
                {
                    var messages = _quene.DeQueenAll(type);
                    var listtype = typeof(List<>).MakeGenericType(new Type[]
                    {
                        typeof(object)
                    });

                    int updatecount = 0;
                    int insertcount = 0;

                    if (messages.Length > 0)
                    {
                        foreach (var submessages in messages.GroupBy(p => ((DateTime)p.GetPropertyValue("CreateTime")).ToString("yyyyMM")))
                        {
                            var updatemessgae = submessages.Where(p => p.GetPropertyValue("ModifyTime") != null && (DateTime)(p.GetPropertyValue("ModifyTime")) != DateTime.MinValue).ToList();
                            var insertmessage = submessages.Except(updatemessgae).ToList();

                            var _bus = ServiceLocator.Instance.GetRequiredService<ID_UserMessageBusiness>();

                            MethodInfo insertListAsync = _bus.GetType().GetMethod("InsertAsync", new Type[] { listtype });
                            Task task = insertListAsync.Invoke(_bus, new Object[] { insertmessage }) as Task;
                            await task;

                            MethodInfo updateListAsync = _bus.GetType().GetMethod("UpdateAsync", new Type[] { listtype });
                            Task task2 = updateListAsync.Invoke(_bus, new Object[] { updatemessgae }) as Task;
                            await task2;


                            updatecount += updatemessgae.Count;
                            insertcount += insertmessage.Count;
                        }


                        EnPushMessageQueen(_quene, messages);
                    }
                    else
                    {
                        logLevel = LogLevel.Trace;
                    }

                    message = $"插入{type.Name}数据{insertcount}条;更新{type.Name}数据{updatecount}条 ";

                }

            }
            catch (Exception ex)
            {
                logLevel = LogLevel.Error;
                message = ex.Message;
            }

            FileQuartz.WriteJobExecute(logLevel, trigger.FullName, message);
        }

        public static void EnPushMessageQueen(IQuene quene, params object[] entityBases)
        {
            List<string> ids = new List<string>();
            foreach (var entityBase in entityBases)
            {
                //加入到队列通知给客户端刷新 
                if (entityBase.ContainsProperty("UserIds"))
                {
                    var str = entityBase.GetPropertyValue("UserIds")?.ToString();
                    ids.AddRange(str.Split(new string[] { "^" }, StringSplitOptions.RemoveEmptyEntries));
                }
            }
            if (ids.Count > 0)
            {
                quene.EnQueen("PushMessageTypes", ids.Distinct().ToArray());
            }

            //if (GlobalData.PushMessageTypes.Contains(typeof(T)) && !GlobalData.BatchSaveTypes.Contains(typeof(T)))
            //{
            //    SaveMessageJob.EnPushMessageQueen(entity);
            //}
        }
    }


}
