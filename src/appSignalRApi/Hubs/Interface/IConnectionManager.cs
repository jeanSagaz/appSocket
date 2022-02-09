using System.Collections.Generic;

namespace appSignalRApi.Hubs.Interface
{
    public interface IConnectionManager
    {
        void AddConnection(string userName, string connectionId);
        void RemoveConnection(string connectionId);
        HashSet<string> GetConnections(string userName);
        IEnumerable<string> OnlineUsers();
    }
}
