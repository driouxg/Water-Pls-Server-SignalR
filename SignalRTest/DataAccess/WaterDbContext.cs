using Microsoft.EntityFrameworkCore;
using SignalRTest.Domain.Dto;

namespace SignalRTest.DataAccess
{
    public class WaterDbContext : DbContext
    {
        public WaterDbContext(DbContextOptions<WaterDbContext> options)
            : base(options)
        {}

        public DbSet<UserDto> Users { get; set; }
    }
}