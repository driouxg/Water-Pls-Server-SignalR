using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RequestWaterHub : Hub
    {
        private readonly ILogger _logger;
        private WaterDbContext _dbContext;
        private ConnectionMap<ApplicationUser> requestorConnections;

        public RequestWaterHub(WaterDbContext dbContext, ILogger<RequestWaterHub> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            requestorConnections = RequestorConnectionSingleton.Instance;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task RequestWater()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "requestors");

            _logger.LogInformation($"Added connection '{Context.ConnectionId}' to 'requestors' group.");
        }

        //public async Task RequestWater(string username)
        //{
        //    //// Convert to Value Object
        //    //ApplicationUser requestorUsername = new ApplicationUser(username);
        //
        //    //if (UserExists(requestorUsername))
        //    //{
        //    //    // Add to requestor set
        //    //    requestorConnections.Add(requestorUsername, Context.ConnectionId);
        //    //
        //    //    // Get current client connection
        //    //    var connectionId = Context.ConnectionId;
        //    //
        //    //    //Context.User.
        //    //
        //    //    // Get current user's coordinates
        //    //    GetUserCoordinates(requestorUsername);
        //    //
        //    //    // Find the closest donator to the current requestor
        //    //    var closestDonator = FindClosestDonator(requestorUsername);
        //    //
        //    //    // Message the donator and requestor that their matches have been found
        //    //    HashSet<string> donatorConnectionStrings = DonatorConnectionSingleton.Instance.GetValue(requestorUsername);
        //    //    //foreach (string donatorString in donatorConnectionStrings)
        //    //    //{
        //    //    //    Clients.User()
        //    //    //}
        //    //}
        //    //else
        //    //{
        //    //    _logger.LogInformation("Username: 'username' does not exist in database.");
        //    //}
        //}

        private UserDto FindClosestDonator(ApplicationUser requestorName)
        {
            // Get the donator hub singleton
            ConnectionMap<ApplicationUser> donatorConnectionMap = DonatorConnectionSingleton.Instance;

            _logger.LogInformation($"Find closest donator to {requestorName}. Searching through {donatorConnectionMap.Count()} active donators.");

            ICollection<UserDto> vals = RetrieveConnectedDonators(donatorConnectionMap.Keys());

            UserDto result = QueryRequestorByName(requestorName);

            return FindTheClosestDonator(result, vals);
        }

        private UserDto QueryRequestorByName(ApplicationUser usernameVo)
        {
            return null;//_dbContext.Users.Single(i => i.Username == usernameVo.value);
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

        private ICollection<UserDto> RetrieveConnectedDonators(ICollection<ApplicationUser> donators)
        {
            return null;
            //return _dbContext.Users.Where(
            //    i => donators.Contains(new UsernameVo(i.Username))
            //).ToList();
        }

        private GeoCoordinatesVo GetUserCoordinates(ApplicationUser usernameVo)
        {
            //var userDto = _dbContext.Users.Single(x => x.Username == usernameVo.value);
            return null; //new GeoCoordinatesVo(userDto.geoCoordinatesDto);
        }

        private bool UserExists(ApplicationUser username)
        {
            return false; //_dbContext.Users.Single(x => x.Username == username.value) != null;
        }
    }
}