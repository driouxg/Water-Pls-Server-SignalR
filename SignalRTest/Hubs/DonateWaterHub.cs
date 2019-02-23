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
using SignalRTest.Singleton;

namespace SignalRTest.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DonateWaterHub : Hub
    {
        private WaterDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private ConnectionMap<ApplicationUser> donatorConnections;

        public DonateWaterHub(WaterDbContext dbContext, ILogger<DonateWaterHub> logger, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _logger = logger;
            _userManager = userManager;
            donatorConnections = DonatorConnectionSingleton.Instance;
        }

        public override Task OnConnectedAsync()
        {
            // Get user from DB
            var user = _userManager.FindByIdAsync(Context.User.Identity.Name);

            _logger.LogInformation($"User's user ID: {user.Id} and user's username: {user.Result.UserName}");

            if (user == null)
            {
                return base.OnConnectedAsync();
            }

            // Check if user is already existent in connection map
            bool userExistsInConnectionMap = donatorConnections.ContainsKey(user.Result);

            // Already exists
            if (userExistsInConnectionMap)
            {
                donatorConnections.AddValueToSet(user.Result, Context.ConnectionId);
            }

            // Doesn't exist
            if (!userExistsInConnectionMap)
            {
                donatorConnections.Add(user.Result, Context.ConnectionId);
            }

            _logger.LogInformation($"User already exists in connection map: {userExistsInConnectionMap}");

            //donatorConnections.Add(nullUser, Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        //private 

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var user = Context.User.Identity.Name;

            //_logger.LogWarning($"User with connectionId: {}");
            //UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
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
