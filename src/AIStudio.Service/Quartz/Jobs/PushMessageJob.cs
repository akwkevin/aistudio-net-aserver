using AIStudio.Service.WebSocketEx;
using Coldairarrow.Business.D_Manage;
using Coldairarrow.Business.OA_Manage;
using Coldairarrow.Business.Quartz_Manage;
using Coldairarrow.Util;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AIStudio.Service.Quartz
{
    public class PushMessageJob : IJob
    {
        ICustomWebSocketFactory _wsFactory { get; }
        ID_UserMessageBusiness _userMessageBusiness { get; }
        ID_UserMailBusiness _userMailBusiness { get; }
        IOA_UserFormBusiness _userFormBusiness { get; }
        IDistributedCache _distributed { get; }

        public PushMessageJob(ICustomWebSocketFactory wsFactory, ID_UserMessageBusiness userMessageBusiness, ID_UserMailBusiness userMailBusiness, IOA_UserFormBusiness userFormBusiness, IDistributedCache distributed)
        {
            _wsFactory = wsFactory;
            _userMessageBusiness = userMessageBusiness;
            _userMailBusiness = userMailBusiness;
            _userFormBusiness = userFormBusiness;
            _distributed = distributed;
        }

        public Task Execute(IJobExecutionContext context)
        {
            Quartz_TaskDTO taskOptions = context.GetTaskOptions();
            string message = string.Empty;
            AbstractTrigger trigger = (context as JobExecutionContextImpl).Trigger as AbstractTrigger;
            if (taskOptions == null)
            {
                FileQuartz.WriteJobExecute(LogLevel.Warning, trigger.FullName, "未到找作业或可能被移除");
                return Task.CompletedTask;
            }

            LogLevel logLevel = LogLevel.Information;
            StringBuilder stringBuilder = new StringBuilder();

            try
            {
                IQuene quene = ServiceLocator.Instance.GetRequiredService<IQuene>();

                var clearcache = quene.DeQueenAll<string>("CachTypes").Distinct().ToArray();

                var pushMessage = quene.DeQueenAll<string>("PushMessageTypes").Distinct().ToArray();

                foreach (var customWebSocket in _wsFactory.All())
                {
                    if (!customWebSocket.IsFirstPushed || pushMessage.Contains(customWebSocket.UserId) || clearcache.Length > 0)
                    {
                        customWebSocket.IsFirstPushed = true;

                        var groupdata = _userMessageBusiness.GetHistoryGroupDataListAsync(new PageInput<D_UserMessageInputDTO>()
                        {
                            PageRows = 0,
                            Search = new D_UserMessageInputDTO()
                            {
                                userId = customWebSocket.UserId,
                                markflag = true,
                            }
                        }).Result;

                       var result2 = _userMailBusiness.GetHistoryDataListAsync(new PageInput<D_UserMailInputDTO>()
                       {
                           PageRows = 0,
                           Search = new D_UserMailInputDTO()
                           {
                               userId = customWebSocket.UserId,
                               markflag = true,
                           }
                       }).Result;

                        //var pagination3 = new Pagination() { PageRows = 0 };
                        //_userFormBusiness.GetHistoryDataListAsync(pagination3, null, null, customWebSocket.UserId, null, null, null).Wait();
                        //查询并发送给客户端
                        //stringBuilder.Append();
                        var send = new MessageResult()
                        {
                            Success = true,
                            Data = new
                            {
                                Clearcache = clearcache,
                                UserMessageCount = groupdata.Sum(p => p.Total),
                                UserMailCount = result2.Total,
                                //UserFormCount = pagination3.Total,
                            },
                            MessageType = WSMessageType.PushType
                        };
                        byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(send));
                        customWebSocket.WebSocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
                    }
                }

                message = stringBuilder.ToString();
                if (string.IsNullOrEmpty(message))
                {
                    logLevel = LogLevel.Trace;
                }
            }
            catch (Exception ex)
            {
                logLevel = LogLevel.Error;
                message = ex.Message;
            }

            FileQuartz.WriteJobExecute(logLevel, trigger.FullName, message);
            return Task.CompletedTask;
        }
    }
}
