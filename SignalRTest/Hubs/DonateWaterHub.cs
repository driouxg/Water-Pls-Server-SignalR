using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRTest.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTest.Hubs
{
    [Authorize]
    public class DonateWaterHub : Hub
    {
        private WaterDbContext _dbContext;
        private readonly ILogger _logger;

        public DonateWaterHub(WaterDbContext dbContext, ILogger<DonateWaterHub> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public override Task OnConnected()
        {
            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            return base.OnDisconnected();
        }

        public async Task DonateWater()
        {
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
            await Clients.Group("requestors").
        }
    }
}
