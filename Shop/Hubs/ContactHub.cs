using Microsoft.AspNetCore.SignalR;

namespace Shop.UI.Hubs
{
    public class ContactHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
