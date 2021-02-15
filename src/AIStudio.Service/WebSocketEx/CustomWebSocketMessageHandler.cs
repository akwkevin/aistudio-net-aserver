using AIStudio.Service;
using Coldairarrow.Business.D_Manage;
using Coldairarrow.Entity.D_Manage;
using Coldairarrow.Util;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AIStudio.Service.WebSocketEx
{
    public interface ICustomWebSocketMessageHandler
    {
        Task SendInitialMessages(CustomWebSocket userWebSocket, string anyText = "#OK");
        Task HandleMessage(WebSocketReceiveResult result, byte[] buffer, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory);
        Task BroadcastOthers(byte[] buffer, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory);
        Task BroadcastAll(byte[] buffer, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory);
        Task BroadcastOthers(string userIds, CustomWebSocket userWebSocket, object message);
    }

    public class CustomWebSocketMessageHandler : ICustomWebSocketMessageHandler
    {
        private readonly ICustomWebSocketFactory _wsFactory;
        private readonly ILogger _logger;
        private readonly IQuene _quene;
        public CustomWebSocketMessageHandler(ICustomWebSocketFactory wsFactory, ILogger<CustomWebSocketMessageHandler> logger, IQuene quene)
        {
            _wsFactory = wsFactory;
            _logger = logger;
            _quene = quene;
        }

        public async Task SendInitialMessages(CustomWebSocket userWebSocket, string anyText = "#OK")
        {
            WebSocket webSocket = userWebSocket.WebSocket;
            MessageResult send = new MessageResult()
            {
                Success = true,
                Data = new D_UserMessageDTO
                {
                    CreateTime = DateTime.Now,
                    Text = anyText,
                    CreatorId = "system",
                    CreatorName = "system",
                    Type = 1,
                },
                MessageType = WSMessageType.Success,
            };

            string serialisedMessage = JsonConvert.SerializeObject(send);
            byte[] bytes = Encoding.ASCII.GetBytes(serialisedMessage);
            await webSocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);

        }

        public async Task HandleMessage(WebSocketReceiveResult result, byte[] buffer, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory)
        {
            string msg = Encoding.UTF8.GetString(buffer.Reverse().SkipWhile(x => x == 0).Reverse().ToArray());
            try
            {
                var message = JsonConvert.DeserializeObject<MessageResult>(msg);

                if (message.MessageType == WSMessageType.MessageType)
                {
                    var data = JsonConvert.DeserializeObject<D_UserMessageDTO>((message.Data ?? "").ToString());
                    if (data == null)
                        return;

                    D_UserMessageDTO senddata = null;
                    if (string.IsNullOrEmpty(data.GroupId))
                    {
                        if (data.UserIds == "^" + wsFactory.GetSmallAssistant().UserId + "^")
                        {
                            data.ReadingMarks = "^" + wsFactory.GetSmallAssistant().UserId + "^";
                            senddata = new D_UserMessageDTO()
                            {
                                Id = IdHelper.GetId(),
                                CreateTime = DateTime.Now,
                                Text = GetCallBack(data.Text),
                                CreatorId = wsFactory.GetSmallAssistant().UserId,
                                CreatorName = wsFactory.GetSmallAssistant().UserName,
                                UserIds = "^" + userWebSocket.UserId + "^",
                                UserNames = "^" + userWebSocket.UserName + "^",
                                Avatar = wsFactory.GetSmallAssistant().Avatar,
                                Role = "Sender",
                                Type = 1,
                            };
                        }
                    }

                    InitEntity(data, userWebSocket);
                    _quene.EnQueen<D_UserMessage>(data);
                    {
                        MessageResult send = new MessageResult()
                        {
                            Success = true,
                            Data = data,
                            MessageType = WSMessageType.MessageType,
                        };
                        byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(send));
                        //数据回发一份，所有界面能同步
                        await userWebSocket.WebSocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                    }

                    if (senddata != null)
                    {
                        MessageResult send = new MessageResult()
                        {
                            Success = true,
                            Data = senddata,
                            MessageType = WSMessageType.MessageType,
                        };
                        byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(send));
                        await userWebSocket.WebSocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);

                        _quene.EnQueen<D_UserMessage>(senddata);
                    }
                    else
                    {
                        await BroadcastOthers(data, userWebSocket, wsFactory);
                    }
                }
                else if (message.MessageType == WSMessageType.ReadMessageType)
                {
                    var data = JsonConvert.DeserializeObject<D_UserMessageDTO>((message.Data ?? "").ToString());
                    if (data == null)
                        return;

                    UpdateEntity(data, userWebSocket);
                    _quene.EnQueen<D_UserMessage>(data);
                }
                else if (message.MessageType == WSMessageType.PingType)
                {
                    await userWebSocket.WebSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                    _logger.LogDebug(UserLogType.WebSocket.ToEventId(), $"收到{userWebSocket.IP}的心跳包{message.Data}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(UserLogType.WebSocket.ToEventId(), ex, ex.Message);
            }

        }

        public async Task BroadcastOthers(D_UserMessageDTO message, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory)
        {
            MessageResult send = new MessageResult()
            {
                Success = true,
                Data = message,
                MessageType = WSMessageType.MessageType,
            };

            await BroadcastOthers(message.UserIds, userWebSocket, send);
        }

        public async Task BroadcastOthers(string userIds, CustomWebSocket userWebSocket, object message)
        {
            string[] receives = userIds.Split(new string[] { "^" }, StringSplitOptions.RemoveEmptyEntries);
            byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            foreach (var userId in receives.Distinct())
            {
                var others = _wsFactory.Client(userId);
                foreach (var other in others)
                {
                    if (other != userWebSocket)//不要发给自己
                    {
                        await other.WebSocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
            }
        }

        public async Task BroadcastOthers(byte[] buffer, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory)
        {
            var others = wsFactory.Others(userWebSocket);
            foreach (var other in others)
            {
                await other.WebSocket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task BroadcastAll(byte[] buffer, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory)
        {
            var all = wsFactory.All();
            foreach (var uws in all)
            {
                await uws.WebSocket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public static string GetCallBack(string recv_data)
        {
            if (recv_data == "quit" || recv_data == "exit" || recv_data == "Bye" || recv_data == "bye" || recv_data == "再见")
            {
                return "再见";
            }
            else if (recv_data.Contains("你好") || recv_data.Contains("hello"))
            {
                return "你好";
            }
            else if (recv_data.Contains("sb") || recv_data.Contains("SB") || recv_data.Contains("傻") || recv_data.Contains("二货"))
            {
                return "你才傻，你全家都傻！！！";
            }
            else if (recv_data.Contains("贱") || recv_data.Contains("蠢"))
            {
                return "你个蠢货！";
            }
            else if (recv_data.Contains("吃") || recv_data.Contains("hello"))
            {
                return "红烧肉、东坡肘子...";
            }
            else if (recv_data.Contains("玩") || recv_data.Contains("hello"))
            {
                return "云南丽江不错！";
            }
            else if (recv_data.Contains("名字") || recv_data.Contains("name"))
            {
                return "我叫小美，编号9527,哈哈...";
            }
            else if (recv_data.Contains("时间") || recv_data.Contains("time"))
            {
                return "现在时间是:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                return "你可以和我聊天哦";
            }
        }

        public static void InitEntity(object entity, CustomWebSocket userWebSocket)
        {
            if (entity.ContainsProperty("Id"))
                entity.SetPropertyValue("Id", IdHelper.GetId());
            if (entity.ContainsProperty("CreateTime"))
                entity.SetPropertyValue("CreateTime", DateTime.Now);
            if (entity.ContainsProperty("CreatorId"))
                entity.SetPropertyValue("CreatorId", userWebSocket?.UserId);
            if (entity.ContainsProperty("CreatorName"))
                entity.SetPropertyValue("CreatorName", userWebSocket?.UserName);
        }

        public static void UpdateEntity(object entity, CustomWebSocket userWebSocket)
        {
            if (entity.ContainsProperty("ModifyTime"))
                entity.SetPropertyValue("ModifyTime", DateTime.Now);
            if (entity.ContainsProperty("ModifyId"))
                entity.SetPropertyValue("ModifyId", userWebSocket?.UserId);
            if (entity.ContainsProperty("ModifyName"))
                entity.SetPropertyValue("ModifyName", userWebSocket?.UserName);
        }
    }
}
