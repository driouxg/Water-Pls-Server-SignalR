using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRTest.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using SignalRTest.Domain;
using SignalRTest.Domain.VO;
using SignalRTest.Singleton;

namespace SignalRTest.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DonateWaterHub : Hub
    {
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private ConnectionMap<UsernameVo> donatorConnections;

        public DonateWaterHub(ILogger<DonateWaterHub> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            donatorConnections = DonatorConnectionSingleton.Instance;
        }

        public override Task OnConnectedAsync()
        {
            UsernameVo username = GetUsernameVoForCurrentConnection();

            _logger.LogInformation($"User's username: {username._value}");

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
            bool userExistsInConnectionMap = donatorConnections.ContainsKey(username);

            // Already exists
            if (userExistsInConnectionMap)
            {
                donatorConnections.AddValueToSet(username, Context.ConnectionId);
            }
            else
            {
                donatorConnections.Add(username, Context.ConnectionId);
            }

            _logger.LogInformation($"Number of users in donatorConnections: {donatorConnections.Count()}");
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
            if (donatorConnections.ContainsKey(username))
            {
                donatorConnections.RemoveValueFromSet(username, Context.ConnectionId);
                _logger.LogInformation($"Removed connectionId '{Context.ConnectionId}' for user '{username._value}'");
            }
        }

        public async Task DonateWater(/*string userId*/)
        {
            //var user = await _userManager.FindByIdAsync(userId);

            Console.WriteLine($"Current user is: {Context.User.Identity.Name}");
            Console.WriteLine();
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
            //await Clients.Group("requestors").
        }
    }
}
