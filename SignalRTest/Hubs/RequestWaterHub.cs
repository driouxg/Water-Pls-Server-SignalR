using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRTest.DataAccess;
using SignalRTest.Domain;
using SignalRTest.Domain.VO;
using SignalRTest.Singleton;

namespace SignalRTest.Hubs
{
    public class RequestWaterHub : Hub
    {
        private readonly ILogger _logger;
        private WaterDbContext _dbContext;
        private ConnectionMap<UsernameVo> requestorConnections;

        public RequestWaterHub(WaterDbContext dbContext)
        {
            _dbContext = dbContext;
            requestorConnections = RequestorConnectionSingleton.Instance;
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
                requestorConnections.Add(usernameVo, Context.ConnectionId);

                // Get current client connection
                var connectionId = Context.ConnectionId;

                // Get the donator hub singleton
                ConnectionMap<UsernameVo> donatorConnectionMap = DonatorConnectionSingleton.Instance;

                // Get current user's coordinates
                GetUserCoordinates(usernameVo);

                // Find the closest donator to the current requestor
                FindClosestDonator(usernameVo);

                // Message the donator and requestor that their matches have been found

            }
            else
            {
                _logger.LogInformation("Username: 'username' does not exist in database.");
            }
        }

        private GeoCoordinatesVo GetUserCoordinates(UsernameVo usernameVo)
        {
            var userDto = _dbContext.Users.Single(x => x.Username == usernameVo.value);
            return new GeoCoordinatesVo(userDto.geoCoordinatesDto);
        }

        private bool UserExists(UsernameVo username)
        {
            return _dbContext.Users.Single(x => x.Username == username.value) != null;
        }
    }
}