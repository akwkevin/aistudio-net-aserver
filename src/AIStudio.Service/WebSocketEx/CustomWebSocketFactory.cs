using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIStudio.Service.WebSocketEx
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICustomWebSocketFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        void Add(CustomWebSocket client);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        void Remove(CustomWebSocket client);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<CustomWebSocket> All();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        List<CustomWebSocket> Others(CustomWebSocket client);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<CustomWebSocket> Client(string userId);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        CustomWebSocket GetSmallAssistant();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<CustomWebSocket> AllWithAssistant();
    }

    /// <summary>
    /// 
    /// </summary>

    public class CustomWebSocketFactory : ICustomWebSocketFactory
    {
        List<CustomWebSocket> List;

        /// <summary>
        /// 
        /// </summary>
        public CustomWebSocketFactory()
        {
            List = new List<CustomWebSocket>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public void Add(CustomWebSocket client)
        {
            List.Add(client);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public void Remove(CustomWebSocket client)
        {
            List.Remove(client);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<CustomWebSocket> All()
        {
            return List.Where(c => c.WebSocket.State == System.Net.WebSockets.WebSocketState.Open).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public List<CustomWebSocket> Others(CustomWebSocket client)
        {
            return List.Where(c => c != client && c.WebSocket.State == System.Net.WebSockets.WebSocketState.Open).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<CustomWebSocket> Client(string userId)
        {     
            return List.Where(c => c.UserId == userId && c.WebSocket.State == System.Net.WebSockets.WebSocketState.Open).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CustomWebSocket GetSmallAssistant()
        {
            return new CustomWebSocket() { UserId = "smallAssistant", UserName = "智能助手", Avatar = "/Images/Usopp.jpg" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<CustomWebSocket> AllWithAssistant()
        {
            return new List<CustomWebSocket>() { GetSmallAssistant() }.Union(List).ToList();
        }
    }
}
