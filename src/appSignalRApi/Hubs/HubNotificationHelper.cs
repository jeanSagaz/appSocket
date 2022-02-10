using appSignalRApi.Hubs.Interface;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace appSignalRApi.Hubs
{
    public class HubNotificationHelper : IHubNotificationHelper
    {
        IHubContext<NotificationHub> _hubContext { get;  }
        private readonly IConnectionManager _connectionManager;

        public HubNotificationHelper(IHubContext<NotificationHub> context,
            IConnectionManager connectionManager)
        {
            _hubContext = context;
            _connectionManager = connectionManager;
        }

        public IEnumerable<string> GetOnlineUsers()
        {
            return _connectionManager.OnlineUsers();
        }

        public async Task<Task> SendNofificationParallel(string userName)
        {
            HashSet<string> connections = _connectionManager.GetConnections(userName);

            try
            {
                if (connections is not null & connections.Count > 0)
                {
                    foreach(var connection in connections)
                    {
                        try
                        {
                            await _hubContext.Clients.Clients(connection).SendAsync("socket", connection);
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

        public async Task SendNofificationToAll(string message)
        {
            await _hubContext.Clients.All.SendAsync("notify", message);
        }
    }
}
