using System.Collections.Generic;

namespace appSignalRApi.Hubs.Interfaces
{
    public interface IHubConnectionHandler
    {
        void AddConnection(string name, string connectionId);
        void RemoveConnection(string connectionId);
        HashSet<string> GetConnections(string name);
        IEnumerable<string> OnlineHubs();
    }
}
