using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRTest.DataAccess;
using SignalRTest.Domain;
using SignalRTest.Domain.Dto;
using SignalRTest.Domain.VO;
using SignalRTest.Services;
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

        private ICollection<UserDto> FindClosestDonator(UsernameVo requesterName)
        {
            // Get the donator hub singleton
            ConnectionMap<UsernameVo> donatorConnectionMap = DonatorConnectionSingleton.Instance;

            _logger.LogInformation($"Find closest donator to {requesterName}. Searching through {donatorConnectionMap.Count()} active donators.");

            var vals = RetrieveConnectedDonators(donatorConnectionMap.Keys());

            var result = QueryRequestorByName(requesterName);

            return FindTheClosestDonator(result , vals);

            return null;
        }

        private UserDto FindTheClosestDonator(UserDto requestor, ICollection<UserDto> donators)
        {
            UserDto closestDonator = null;
            double shortestDistance = Single.NaN;
            double distance = Single.NaN;
            LocationDistanceCalculator calculator = null;

            foreach (var donator in donators)
            {
                calculator = new LocationDistanceCalculator(new GeoCoordinatesVo(requestor.geoCoordinatesDto), new GeoCoordinatesVo(donator.geoCoordinatesDto));

                distance = calculator.CalculateDistanceInMiles();

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestDonator = donator;
                }
            }

            return closestDonator;
        }

        private ICollection<UserDto> RetrieveConnectedDonators(ICollection<UsernameVo> donators)
        {
            return _dbContext.Users.Where(
                i => donators.Contains(new UsernameVo(i.Username))
            ).ToList();
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