using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRTest.Hubs
{
    public class MessageHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}