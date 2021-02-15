using Coldairarrow.Business.Quartz_Manage;
using Coldairarrow.Util;
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
        public Task Execute(IJobExecutionContext context)
        {
            Quartz_TaskDTO taskOptions = context.GetTaskOptions();
            string message = "";
            AbstractTrigger trigger = (context as JobExecutionContextImpl).Trigger as AbstractTrigger;
            if (taskOptions == null)
            {             
                FileQuartz.WriteJobExecute(LogLevel.Warning, trigger.FullName , "未到找作业或可能被移除");
                return Task.CompletedTask;
            }

            LogLevel logLevel = LogLevel.Information;
            try
            {           
                //foreach (var type in GlobalData.BatchSaveTypes)
                //{
                //    var messages = CacheHelper.Cache.DeQueenAll(type);
                //    var listtype = typeof(List<>).MakeGenericType(new Type[]
                //    {
                //        typeof(object)
                //    });

                //    int updatecount = 0;
                //    int insertcount = 0;
                //    //改成反射，不在固定类
                //    //var messages = CacheHelper.Cache.DeQueenAll<D_UserMessage>();
                //    if (messages.Length > 0)
                //    {
                //        foreach (var submessages in messages.GroupBy(p => ((DateTime)p.GetPropertyValue("CreateTime")).ToString("yyyyMM")))
                //        {
                //            var updatemessgae = submessages.Where(p => p.GetPropertyValue("ModifyTime") != null && (DateTime)(p.GetPropertyValue("ModifyTime")) != DateTime.MinValue).ToList();
                //            var insertmessage = submessages.Except(updatemessgae).ToList();

                //            List<TableMapperRule> rules = new List<TableMapperRule>()
                //            {
                //                new TableMapperRule()
                //                {
                //                    MappingType = type,
                //                    TableName = type.Name + submessages.Key,
                //                }
                //            };
                //            using (IRepository _bus = DbFactory.GetRepository(null, null, rules))
                //            {
                //                //_bus.InsertAsync<D_UserMessage>(submessages.ToList()).Wait();
                //                MethodInfo insertListAsync = _bus.GetType().GetMethod("InsertAsync", new Type[] { listtype }) ;
                //                Task task = insertListAsync.Invoke(_bus, new Object[] { insertmessage }) as Task;
                //                task.Wait();

                //                MethodInfo updateListAsync = _bus.GetType().GetMethod("UpdateAsync", new Type[] { listtype });
                //                Task task2 = updateListAsync.Invoke(_bus, new Object[] { updatemessgae }) as Task;
                //                task2.Wait();
                //            }

                //            updatecount += updatemessgae.Count;
                //            insertcount += insertmessage.Count;
                //            //#region 获取对应程序集及服务
                //            //var businesstype = GlobalData.FxAllTypes.FirstOrDefault(p => p.Name == "I" + type.Name + "Business");
                //            //if (businesstype == null)
                //            //{
                //            //    throw new Exception("没有该数据类型的处理方法！");
                //            //}

                //            //var business = AutofacHelper.GetService(businesstype);
                //            //if (business == null)
                //            //{
                //            //    throw new Exception("该数据类型的处理方法没有注册！");
                //            //}
                //            //#endregion

                //            //#region 反射获取数据方法
                //            //MethodInfo insertListAsync = business.GetType().GetMethod("InsertAsync", new Type[] { listtype, typeof(string) });
                //            //Task task = insertListAsync.Invoke(business, new Object[] { submessages.ToList(), submessages.Key }) as Task;
                //            //task.Wait();
                //            //#endregion
                //        }

                       
                //        GlobalData.EnPushMessageQueen(messages);
                //    }
                //    else
                //    {
                //        logLevel = Util.LogLevel.Trace;
                //    }

                //    message = $"插入{type.Name}数据{insertcount}条;更新{type.Name}数据{updatecount}条 ";

                //}
                
            }
            catch (Exception ex)
            {
                logLevel = LogLevel.Error;
                message = ex.Message;
            }

            FileQuartz.WriteJobExecute(logLevel, trigger.FullName , message);
            return Task.CompletedTask;
        }
    }
}
