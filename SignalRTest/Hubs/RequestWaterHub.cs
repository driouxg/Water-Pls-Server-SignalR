using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private ConnectionMap<UsernameVo> requestorConnections;
    

        public RequestWaterHub(ILogger<RequestWaterHub> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            requestorConnections = RequestorConnectionSingleton.Instance;
        }

        public async Task RequestWater()
        {
            UsernameVo username = GetUsernameVoForCurrentConnection();

            Console.WriteLine($"Current user is: {username._value}");

            await Groups.AddToGroupAsync(username._value, "requestors");

            _logger.LogInformation($"Added user '{username._value}' to 'requestors' group.");


            // Let's try just sending a message to the requestor first
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", "Hi there partner");
            await Clients.All.SendAsync("ReceiveMessage", "Sending message to all users.");
        }

        public override Task OnConnectedAsync()
        {
            UsernameVo username = GetUsernameVoForCurrentConnection();

            _logger.LogInformation($"User '{username._value}' connected with connectionId '{Context.ConnectionId}'");

            if (username == null)
            {
                return base.OnConnectedAsync();
            }

            AddUserToConnectionMap(username);

            return base.OnConnectedAsync();
        }

        private void AddUserToConnectionMap(UsernameVo username)
        {
            // Check if user is already existent in connection map
            if (requestorConnections.ContainsKey(username))
            {
                requestorConnections.AddValueToSet(username, Context.ConnectionId);
            }
            else
            {
                requestorConnections.Add(username, Context.ConnectionId);
            }

            _logger.LogInformation($"Number of users in donatorConnections: {requestorConnections.Count()}");
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            UsernameVo username = GetUsernameVoForCurrentConnection();

            if (username == null)
            {
                return base.OnConnectedAsync();
            }

            RemoveUserFromConnectionMap(username);

            return base.OnDisconnectedAsync(exception);
        }

        private UsernameVo GetUsernameVoForCurrentConnection()
        {
            ApplicationUser user = _userManager.FindByIdAsync(Context.User.Identity.Name).Result;
            return new UsernameVo(user.UserName);
        }

        private void RemoveUserFromConnectionMap(UsernameVo username)
        {
            if (requestorConnections.ContainsKey(username))
            {
                requestorConnections.RemoveValueFromSet(username, Context.ConnectionId);
                _logger.LogInformation($"Removed connectionId '{Context.ConnectionId}' for user '{username._value}'");
            }
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
        //    //    HashSet<string> donatorConnectionStrings = DonatorConnectionSingleton.Instance.GetValues(requestorUsername);
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

        //private UserDto FindClosestDonator(ApplicationUser requestorName)
        //{
        //    // Get the donator hub singleton
        //    ConnectionMap<UsernameVo> donatorConnectionMap = DonatorConnectionSingleton.Instance;
        //
        //    _logger.LogInformation($"Find closest donator to {requestorName}. Searching through {donatorConnectionMap.Count()} active donators.");
        //
        //    ICollection<UsernameVo> vals = RetrieveConnectedDonators(donatorConnectionMap.Keys());
        //
        //    UserDto result = QueryRequestorByName(requestorName);
        //
        //    return FindTheClosestDonator(result, vals);
        //}

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