using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using SignalRTest.Domain;
using SignalRTest.Domain.Dto;
using SignalRTest.Domain.VO;
using SignalRTest.Singleton;

namespace SignalRTest.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DonateWaterHub : Hub
    {
        private readonly ILogger _logger;
        private readonly IHubContext<RequestWaterHub> _requestHubContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private ConnectionMap<UsernameVo> _donatorConnections;
        private ConnectionMap<UsernameVo> _requestorConnections;

        public DonateWaterHub(ILogger<DonateWaterHub> logger, IHubContext<RequestWaterHub> requestHubContext, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _requestHubContext = requestHubContext;
            _userManager = userManager;
            _donatorConnections = DonatorConnectionSingleton.Instance;
            _requestorConnections = RequestorConnectionSingleton.Instance;
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
            if (_donatorConnections.ContainsKey(username))
            {
                _donatorConnections.AddValueToSet(username, Context.ConnectionId);
            }
            else
            {
                _donatorConnections.Add(username, Context.ConnectionId);
            }

            _logger.LogInformation($"Number of users in _donatorConnections: {_donatorConnections.Count()}");
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
            if (_donatorConnections.ContainsKey(username))
            {
                _donatorConnections.RemoveValueFromSet(username, Context.ConnectionId);
                _logger.LogInformation($"Removed connectionId '{Context.ConnectionId}' for user '{username._value}'");
            }
        }

        public async Task Hello(GeoCoordinatesDto message)
        {
            _logger.LogInformation($"Received message back from client '{message.latitude}' -----------------");


        }

        public async Task GetListOfCandidates()
        {

        }

        public async Task DonateWaterToRequestor()
        {

        }

        public async Task DonateWater()
        {
            UsernameVo username = GetUsernameVoForCurrentConnection();

            _logger.LogInformation($"User '{username._value}' is looking for requestors in need of water.");
            await Groups.AddToGroupAsync(username._value, "donators");

            UsernameVo requestor = new UsernameVo("drybar21");

            if (_requestorConnections.ContainsKey(requestor))
            {
                _logger.LogInformation($"Sending message to requestor '{requestor}'s connections.");

                // Get their connection Id(s)
                var connectionIds = _requestorConnections.GetValues(requestor);

                foreach (var connectionId in connectionIds)
                {
                    _logger.LogInformation($"The requestor: drybar21 has requested water, we will now send a message to his connectionId '{connectionId}' him from the donator.");
                    await _requestHubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage",
                        $"Donator '{username._value}' has sent you a message!");
                }
            }
        }
    }
}
