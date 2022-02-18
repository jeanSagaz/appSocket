using appSignalRApi.Hubs.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace appSignalRApi.Hubs
{
    public class HubNotificationQueryHelper : IHubNotificationQueryHelper
    {
        IHubContext<NotificationQueryHub> _hubContext { get;  }
        private readonly IHubConnectionHandler _connectionHandler;

        public HubNotificationQueryHelper(IHubContext<NotificationQueryHub> context,
            IHubConnectionHandler connectionHandler)
        {
            _hubContext = context;
            _connectionHandler = connectionHandler;
        }

        public IEnumerable<string> GetOnlineHubs()
        {
            return _connectionHandler.OnlineHubs();
        }

        public async Task<Task> SendNofificationParallel(string name)
        {
            HashSet<string> connections = _connectionHandler.GetConnections(name);

            try
            {
                if (connections is not null & connections.Count > 0)
                {
                    foreach(var connection in connections)
                    {
                        try
                        {
                            await _hubContext.Clients.Clients(connection).SendAsync("notify", connection);
                        }
                        catch
                        {
                            throw;
                        }
                    }
                }

                return Task.CompletedTask;
            }
            catch
            {
                throw;
            }
        }

        public async Task SendNofificationToAll(string message) => await _hubContext.Clients.All.SendAsync("notify", message);

        public async Task SendPrivateNofification(string connectionId, string message) => await _hubContext.Clients.Client(connectionId).SendAsync("notify", message);

        public async Task SendPrivateGroupNofification(string connectionId, string message) => await _hubContext.Clients.Group(connectionId).SendAsync("notify", message);
    }
}
