using Microsoft.AspNetCore.SignalR;
using SignalRTest.DataAccess;
using SignalRTest.Domain;
using SignalRTest.Domain.VO;
using SignalRTest.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTest.Hubs
{
    public class DonateWaterHub : Hub
    {
        private WaterDbContext _dbContext;
        private ConnectionMap<UsernameVo> donatorConnections;

        public DonateWaterHub(WaterDbContext dbContext)
        {
            _dbContext = dbContext;
            donatorConnections = DonatorConnectionSingleton.Instance;
        }
    }
}
