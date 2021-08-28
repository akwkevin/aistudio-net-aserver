using Coldairarrow.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
    public static class WebSocketExtensions
    {
        public static IApplicationBuilder UseCustomWebSocketManager(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CustomWebSocketManager>();
        }
    }

    public class CustomWebSocketManager
    {
        private readonly RequestDelegate _next;

        public CustomWebSocketManager(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ICustomWebSocketFactory wsFactory, ICustomWebSocketMessageHandler wsmHandler, ILogger<CustomWebSocketManager> logger)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    string userName = context.Request.Query["userName"];
                    string userId = context.Request.Query["userId"];
                    string avatar = context.Request.Query["avatar"];
                    if (!string.IsNullOrEmpty(userName))
                    {
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                        if (string.IsNullOrEmpty(ip))
                        {
                            ip = context.Response.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
                        }

                        CustomWebSocket userWebSocket = new CustomWebSocket()
                        {
                            WebSocket = webSocket,
                            UserName = userName,
                            UserId = userId,
                            Avatar = avatar,
                            IP = ip,                            
                            ConnectedTime = DateTime.Now,
                        };
                        wsFactory.Add(userWebSocket);
                        //await wsmHandler.SendInitialMessages(userWebSocket);

                        //通知所有客户端，更新在线用户列表
                        MessageResult send = new MessageResult()
                        {
                            Success = true,
                            Data = wsFactory.All(),
                            MessageType = WSMessageType.OnlineUser,
                        };                  
                        byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(send));
                        await wsmHandler.BroadcastAll(bytes, wsFactory);

                        try
                        {
                            logger.LogDebug(UserLogType.WebSocket.ToEventId(), $"{userWebSocket.UserName}-{userWebSocket.IP}连接成功");
                            await Listen(context, userWebSocket, wsFactory, wsmHandler);

                        }
                        catch (Exception ex)
                        {
                            logger.LogDebug(UserLogType.WebSocket.ToEventId(), $"{userWebSocket.UserName}-{userWebSocket.IP}{ex.Message}");
                        }
                        finally
                        {
                            wsFactory.Remove(userWebSocket);
                            logger.LogDebug(UserLogType.WebSocket.ToEventId(), $"{userWebSocket.UserName}-{userWebSocket.IP}连接关闭");
                        }
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            await _next(context);
        }

        private async Task Listen(HttpContext context, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory, ICustomWebSocketMessageHandler wsmHandler)
        {
            WebSocket webSocket = userWebSocket.WebSocket;
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await wsmHandler.HandleMessage(result, buffer, userWebSocket, wsFactory);
                buffer = new byte[1024 * 4];
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            wsFactory.Remove(userWebSocket);
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);

            //通知所有客户端，更新在线用户列表
            MessageResult send = new MessageResult()
            {
                Success = true,
                Data = wsFactory.All(),
                MessageType = WSMessageType.OnlineUser,
            };
            byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(send));
            await wsmHandler.BroadcastAll(bytes, wsFactory);
        }
    }
}
