using System.Collections.Generic;
using System.Threading.Tasks;

namespace appSignalRApi.Hubs.Interfaces
{
    public interface IHubNotificationQueryHelper
    {
        Task SendNofificationToAll(string message);

        IEnumerable<string> GetOnlineHubs();

        Task<Task> SendNofificationParallel(string name);

        Task SendPrivateNofification(string connectionId, string message);

        Task SendPrivateGroupNofification(string connectionId, string message);
    }
}
