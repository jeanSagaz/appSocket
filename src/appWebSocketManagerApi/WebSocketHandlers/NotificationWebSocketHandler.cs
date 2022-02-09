using appWebSocketManagerApi.Models;
using System.Net.WebSockets;
using System.Threading.Tasks;
using WebSocketManager;
using WebSocketManager.Common;

namespace appWebSocketManagerApi.WebSocketHandlers
{
    public class NotificationWebSocketHandler : WebSocketHandler
    {
        public NotificationWebSocketHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
        }

        public async Task SendMessageBroadcastAsync(WebSocketResponse response)
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

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            var socketId = WebSocketConnectionManager.GetId(socket);

            var message = new Message()
            {
                MessageType = MessageType.Text,
                Data = $"{socketId} is now connected"
            };
            await SendMessageToAllAsync(message);
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            var socketId = WebSocketConnectionManager.GetId(socket);

            await base.OnDisconnected(socket);

            var message = new Message()
            {
                MessageType = MessageType.Text,
                Data = $"{socketId} disconnected"
            };
            await SendMessageToAllAsync(message);
        }
    }
}
