using Microsoft.EntityFrameworkCore;

namespace SignalRTest.DataAccess
{
    public class WaterDbContext : DbContext
    {
        public WaterDbContext(DbContextOptions<WaterDbContext> options)
            : base(options)
        {}


    }
}