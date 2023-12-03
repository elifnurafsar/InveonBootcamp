using Microsoft.AspNetCore.SignalR;

namespace Inveon.Services.SignalRAPI.Hubs
{
    public class MessageHub : Hub
    {

        public override Task OnConnectedAsync()
        {

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

    }
}
