using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace SignalRTest.Hubs
{
    public class RequestWaterHub : Hub
    {
        private readonly ILogger _logger;

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task RequestWater(string username)
        {
            // Convert to Value Object


            var connectionId = Context.ConnectionId;
            // Get 
        }

        private bool UserExists(s)
        {

        }
    }
}