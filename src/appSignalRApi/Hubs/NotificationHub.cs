using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace appSignalRApi.Hubs
{
    public class NotificationHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public string GetConnectionId() => Context.ConnectionId;

        public async Task SendMessage(string user, string message) => await Clients.All.SendAsync("sendMessage", user, message);

        public async Task NewMessage(string userName, string message) => await Clients.All.SendAsync("newMessage", userName, message);

        public async Task BroadcastToConnection(string data, string connectionId) => await Clients.Client(connectionId).SendAsync("broadcasttoclient", data);
    }
}
