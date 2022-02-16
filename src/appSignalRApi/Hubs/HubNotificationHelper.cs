using appSignalRApi.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace appSignalRApi.Hubs
{
    public class HubNotificationHelper : IHubNotificationHelper
    {
        IHubContext<NotificationHub> _hubContext { get;  }
        private readonly IHubConnectionHandler _connectionHandler;

        public HubNotificationHelper(IHubContext<NotificationHub> context,
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
    }
}
