using appWebSocketManagerApi.WebSocketHandlers.Interfaces;
using System.Collections.Generic;

namespace appWebSocketManagerApi.WebSocketHandlers
{
    public class WebSocketManagement : IWebSocketManagement
    {
        private static Dictionary<string, HashSet<string>> socketMap = new Dictionary<string, HashSet<string>>();

        public void AddConnection(string name, string connectionId)
        {
            lock (socketMap)
            {
                if (!socketMap.ContainsKey(name))
                {
                    socketMap[name] = new HashSet<string>();
                }
                socketMap[name].Add(connectionId);
            }
        }

        public void RemoveConnection(string connectionId)
        {
            lock (socketMap)
            {
                socketMap.Remove(connectionId);
            }
        }

        public HashSet<string> GetConnections(string name)
        {
            var conn = new HashSet<string>();

            try
            {
                lock (socketMap)
                {
                    conn = socketMap[name];
                }
            }
            catch
            {
                conn = null;
            }

            return conn;
        }

        public IEnumerable<string> OnlineWebSockets()
        {
            return socketMap.Keys;
        }
    }
}
