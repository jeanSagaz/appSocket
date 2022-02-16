using appSignalRApi.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace appSignalRApi.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly IHubConnectionHandler _hubConnectionHandler;
        private readonly List<Message> _messages;

        public NotificationHub(IHubConnectionHandler hubConnectionHandler)
        {
            _hubConnectionHandler = hubConnectionHandler;

            if (_messages is null)
                _messages = new List<Message>();
        }

        public override Task OnConnectedAsync()
        {
            _hubConnectionHandler.AddConnection(GetConnectionId(), GetConnectionId());
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _hubConnectionHandler.RemoveConnection(GetConnectionId());
            return base.OnDisconnectedAsync(exception);
        }

        public string GetConnectionId() => Context.ConnectionId;

        public async Task NewMessage(string userName, string text)
        {
            await Clients.All.SendAsync("newMessage", userName, text);
            _messages.Add(new Message() 
            {
                UserName = userName,
                Text = text
            });
        }

        public async Task NewUser(string userName, string connectionId)
        {
            await Clients.Client(connectionId).SendAsync("previousMessage", _messages);
            await Clients.All.SendAsync("newUser", userName);
        }
    }

    public class Message
    {
        public string UserName { get; set; }
        public string Text { get; set; }
    }
}
