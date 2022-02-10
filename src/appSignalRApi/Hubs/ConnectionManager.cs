using appSignalRApi.Hubs.Interface;
using System.Collections.Generic;

namespace appSignalRApi.Hubs
{
    public class ConnectionManager : IConnectionManager
    {
        private static Dictionary<string, HashSet<string>> userMap = new Dictionary<string, HashSet<string>>();

        public void AddConnection(string userName, string connectionId)
        {
            lock (userMap)
            {
                if (!userMap.ContainsKey(userName))
                {
                    userMap[userName] = new HashSet<string>();
                }
                userMap[userName].Add(connectionId);
            }
        }

        public void RemoveConnection(string connectionId)
        {
            lock (userMap)
            {
                foreach (var userName in userMap.Keys)
                {
                    if (userMap.ContainsKey(userName))
                    {
                        if (userMap[userName].Contains(connectionId))
                        {
                            userMap[userName].Remove(connectionId);
                            break;
                        }                        
                    }
                }                
            }
        }

        public HashSet<string> GetConnections(string userName)
        {
            var conn = new HashSet<string>();

            try
            {
                lock(userMap)
                {
                    conn = userMap[userName];
                }
            }
            catch
            {
                conn = null;
            }

            return conn;
        }

        public IEnumerable<string> OnlineUsers()
        {
            return userMap.Keys;
        }
    }
}
