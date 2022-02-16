using appSignalRApi.Hubs.Interfaces;
using System.Collections.Generic;

namespace appSignalRApi.Hubs
{
    public class HubConnectionHandler : IHubConnectionHandler
    {
        private static Dictionary<string, HashSet<string>> hubMap = new Dictionary<string, HashSet<string>>();

        public void AddConnection(string name, string connectionId)
        {
            lock (hubMap)
            {
                if (!hubMap.ContainsKey(name))
                {
                    hubMap[name] = new HashSet<string>();
                }
                hubMap[name].Add(connectionId);
            }
        }

        public void RemoveConnection(string connectionId)
        {
            lock (hubMap)
            {
                hubMap.Remove(connectionId);
            }
        }

        public HashSet<string> GetConnections(string name)
        {
            var conn = new HashSet<string>();

            try
            {
                lock(hubMap)
                {
                    conn = hubMap[name];
                }
            }
            catch
            {
                conn = null;
            }

            return conn;
        }

        public IEnumerable<string> OnlineHubs()
        {
            return hubMap.Keys;
        }
    }
}
