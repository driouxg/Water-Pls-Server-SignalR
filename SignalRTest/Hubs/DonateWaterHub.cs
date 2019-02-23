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

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
