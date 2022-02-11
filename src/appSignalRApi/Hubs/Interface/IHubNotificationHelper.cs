using System.Collections.Generic;
using System.Threading.Tasks;

namespace appSignalRApi.Hubs.Interface
{
    public interface IHubNotificationHelper
    {
        Task SendNofificationToAll(string message);

        IEnumerable<string> GetOnlineHubs();

        Task<Task> SendNofificationParallel(string userName);

        Task SendPrivateNofification(string connectionId, string message);
    }
}
