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
        ICustomWebSocketFactory _wsFactory { get { return ServiceLocator.Instance.GetRequiredService<ICustomWebSocketFactory>(); } }
        ID_UserMessageBusiness _userMessageBusiness { get { return ServiceLocator.Instance.GetRequiredService<ID_UserMessageBusiness>(); } }
        ID_UserMailBusiness _userMailBusiness { get { return ServiceLocator.Instance.GetRequiredService<ID_UserMailBusiness>(); } }
        ID_NoticeBusiness _noticeBusiness { get { return ServiceLocator.Instance.GetRequiredService<ID_NoticeBusiness>(); } }
        IOA_UserFormBusiness _userFormBusiness { get { return ServiceLocator.Instance.GetRequiredService<IOA_UserFormBusiness>(); } }
        IDistributedCache _distributed { get { return ServiceLocator.Instance.GetRequiredService<IDistributedCache>(); } }

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

                        var result1 = _userMessageBusiness.GetHistoryDataCountAsync(new Input<D_UserMessageInputDTO>()
                        {
                            Search = new D_UserMessageInputDTO()
                            {
                                userId = customWebSocket.UserId,
                                markflag = true,
                            }
                        }).Result;

                        var result2 = _userMailBusiness.GetHistoryDataCountAsync(new Input<D_UserMailInputDTO>()
                        {
                            Search = new D_UserMailInputDTO()
                            {
                                userId = customWebSocket.UserId,
                                markflag = true,
                            }
                        }).Result;

                        var result3 = _userFormBusiness.GetHistoryDataCountAsync(new Input<OA_UserFormInputDTO>()
                        {
                            Search = new OA_UserFormInputDTO()
                            {
                                userId = customWebSocket.UserId,
                            }
                        }).Result;

                        var result4 = _noticeBusiness.GetHistoryDataCountAsync(new Input<D_NoticeInputDTO>()
                        {
                            Search = new D_NoticeInputDTO()
                            {
                                userId = customWebSocket.UserId,
                                markflag = true,
                            }
                        }).Result;
                        //查询并发送给客户端
                        var send = new MessageResult()
                        {
                            Success = true,
                            Data = new
                            {
                                Clearcache = clearcache,
                                UserMessageCount = result1,
                                UserMailCount = result2,
                                UserFormCount = result3,
                                NoticeCount = result4,
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
