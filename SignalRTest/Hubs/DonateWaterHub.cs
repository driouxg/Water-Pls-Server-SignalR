using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRTest.DataAccess;
using SignalRTest.Domain;
using SignalRTest.Domain.VO;
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
        private ConnectionMap<UsernameVo> donatorConnections;

        public DonateWaterHub(WaterDbContext dbContext, ILogger<DonateWaterHub> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


    }
}
