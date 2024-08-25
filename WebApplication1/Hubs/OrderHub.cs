using Microsoft.AspNetCore.SignalR;

namespace WebApplication1.Hubs
{
    public class OrderHub : Hub
    {
        public async Task NotifyNewOrder()
        {
            await Clients.All.SendAsync("ReceiveOrderNotification");
        }
    }
}
