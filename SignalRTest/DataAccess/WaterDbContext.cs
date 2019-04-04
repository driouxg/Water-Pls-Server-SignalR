using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SignalRTest.Domain;
using SignalRTest.Domain.Entity;

namespace SignalRTest.DataAccess
{
    public class WaterDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public WaterDbContext(DbContextOptions<WaterDbContext> options)
            : base(options)
        { }

        public DbSet<GeoCoordinatesEntity> GeoCoordinates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}