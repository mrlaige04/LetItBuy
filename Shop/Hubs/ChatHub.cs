using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Shop.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        void SendMessage(string user, string message)
        {
            Clients.All.SendAsync("ReceiveMessage", user, message);
            //Clients.All.SendCoreAsync();
            //Clients.Others.SendAsync();
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {      
            return base.OnDisconnectedAsync(exception);
        }
    }
}
