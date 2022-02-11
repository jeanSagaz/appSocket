using appSignalRApi.Hubs.Interface;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace appSignalRApi.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly IHubConnectionHandler _hubConnectionHandler;

        public NotificationHub(IHubConnectionHandler hubConnectionHandler)
        {
            _hubConnectionHandler = hubConnectionHandler;
        }

        public override Task OnConnectedAsync()
        {
            _hubConnectionHandler.AddConnection(GetConnectionId(), GetConnectionId());
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _hubConnectionHandler.RemoveConnection(GetConnectionId());
            return base.OnDisconnectedAsync(exception);
        }

        public string GetConnectionId() => Context.ConnectionId;

        //public async Task SendNotify(string message) => await Clients.All.SendAsync("notify", message);

        //public async Task SendMessage(string user, string message) => await Clients.All.SendAsync("notify", user, message);

        //public async Task NewMessage(string userName, string message) => await Clients.All.SendAsync("notify", userName, message);

        //public async Task BroadcastToConnection(string data, string connectionId) => await Clients.Client(connectionId).SendAsync("broadcasttoclient", data);
    }
}
