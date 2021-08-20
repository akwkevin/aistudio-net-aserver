using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace AIStudio.Service.WebSocketEx
{
    public class CustomWebSocket
    {
        public WebSocket WebSocket { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }

        public string Avatar { get; set; }

        public bool IsFirstPushed { get; set; }
        public string IP { get; set; }

        public DateTime ConnectedTime { get; set; }
       
    }

    
}
