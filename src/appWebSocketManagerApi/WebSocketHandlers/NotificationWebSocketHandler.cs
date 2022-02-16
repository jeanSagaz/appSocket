using appWebSocketManagerApi.Models;
using appWebSocketManagerApi.WebSocketHandlers.Interfaces;
using System.Net.WebSockets;
using System.Threading.Tasks;
using WebSocketManager;
using WebSocketManager.Common;

namespace appWebSocketManagerApi.WebSocketHandlers
{
    public class NotificationWebSocketHandler : WebSocketHandler
    {
        private readonly IWebSocketManagement _webSocketManager;
        public NotificationWebSocketHandler(IWebSocketManagement webSocketManager, 
            WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
            _webSocketManager = webSocketManager;
        }

        public async Task SendMessageBroadcastAsync(WebSocketMessage response)
        {
            await InvokeClientMethodToAllAsync("webSocketMessage", response);
        }

        public async Task SendPrivateMessageAsync(string socketId, string message)
        {
            System.Console.WriteLine($"SendToClient ({socketId}): {message}");
            await InvokeClientMethodAsync(socketId, "webSocketMessage", new object[] {
                new {
                    Message = message
                }}
            );
        }

        public string GetConnectionId(WebSocket socket)
        {
            var socketId = WebSocketConnectionManager.GetId(socket);
            return socketId;
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            var socketId = WebSocketConnectionManager.GetId(socket);

            var message = new Message()
            {
                MessageType = MessageType.Text,
                Data = $"{socketId} is now connected"
            };
            _webSocketManager.AddConnection(socketId, socketId);
            //await SendMessageToAllAsync(message);
            await SendMessageAsync(socketId, message);
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            var socketId = WebSocketConnectionManager.GetId(socket);
            await base.OnDisconnected(socket);

            _webSocketManager.RemoveConnection(socketId);

            var message = new Message()
            {
                MessageType = MessageType.Text,
                Data = $"{socketId} disconnected"
            };            
            await SendMessageToAllAsync(message);
        }
    }
}
