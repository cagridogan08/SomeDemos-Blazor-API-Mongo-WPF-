using Microsoft.AspNetCore.SignalR;

namespace MongoApi.Hub
{
    public class MessageHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public string GetConnectionId() => Context.ConnectionId;

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Send", $"{Context.ConnectionId} joined");
            await base.OnConnectedAsync();
        }
    }
}
