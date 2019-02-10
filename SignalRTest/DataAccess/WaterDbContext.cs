using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SignalRTest.Domain;
using SignalRTest.Domain.Dto;

namespace SignalRTest.DataAccess
{
    public class WaterDbContext : DbContext
    {
        public WaterDbContext(DbContextOptions<WaterDbContext> options)
            : base(options)
        {}

        public DbSet<UserDto> Users { get; set; }
        public DbSet<ApplicationUser> IdManagementUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDto>()
                .Property(b => b.Username)
                .IsRequired();
        }
    }
}