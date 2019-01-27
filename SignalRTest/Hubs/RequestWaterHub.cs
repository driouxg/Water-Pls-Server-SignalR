using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRTest.DataAccess;
using SignalRTest.Domain.VO;
using System.Collections.Generic;

namespace SignalRTest.Hubs
{
    public class RequestWaterHub : Hub
    {
        private readonly ILogger _logger;
        private WaterDbContext _dbContext;
        private Dictionary<string, UsernameVo> requestorMap = new Dictionary<string, UsernameVo>();

        public RequestWaterHub(WaterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task RequestWater(string username)
        {
            // Convert to Value Object
            UsernameVo usernameVo = new UsernameVo(username);

            if (UserExists(usernameVo))
            {
                // Add to requestor set
                requestorMap.Add(Context.ConnectionId, usernameVo);

                // Get current client connection
                var connectionId = Context.ConnectionId;

                // Find the closest donator to the current requestor


                // Message the donator and requestor that their matches have been found
            }
            else
            {
                _logger.LogInformation("Username: 'username' does not exist in database.");
            }
        }

        private bool UserExists(UsernameVo username)
        {
            var result = _dbContext.Users.Single(x => x.Username == username.value);
            return result != null;
        }
    }
}