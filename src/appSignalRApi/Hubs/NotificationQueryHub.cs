using appSignalRApi.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace appSignalRApi.Hubs
{
    public class NotificationQueryHub : Hub
    {
        private readonly IHubConnectionHandler _hubConnectionHandler;

        public NotificationQueryHub(IHubConnectionHandler hubConnectionHandler) => _hubConnectionHandler = hubConnectionHandler;

        public override Task OnConnectedAsync()
        {            
            _hubConnectionHandler.AddConnection(GetConnectionId(), GetConnectionId());

            var sessionId = Context.GetHttpContext().Request.Query["sessionId"];
            return Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _hubConnectionHandler.RemoveConnection(GetConnectionId());
            return base.OnDisconnectedAsync(exception);
        }

        public string GetConnectionId() => Context.ConnectionId;
    }
}
