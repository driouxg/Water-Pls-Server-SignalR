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
        public DbSet<ClientConnectionDto> ClientConnections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDto>()
                .Property(b => b.Username)
                .IsRequired();
        }
    }
}