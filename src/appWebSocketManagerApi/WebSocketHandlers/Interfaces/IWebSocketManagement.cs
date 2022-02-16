using System.Collections.Generic;

namespace appWebSocketManagerApi.WebSocketHandlers.Interfaces
{
    public interface IWebSocketManagement
    {
        void AddConnection(string name, string connectionId);
        void RemoveConnection(string connectionId);
        HashSet<string> GetConnections(string name);
        IEnumerable<string> OnlineWebSockets();
    }
}
