using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SignalRTest.Domain;
using SignalRTest.Domain.Dto;
using SignalRTest.Domain.Entity;

namespace SignalRTest.DataAccess
{
    public class WaterDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public WaterDbContext(DbContextOptions<WaterDbContext> options)
            : base(options)
        { }

        //public DbSet<IdentityUserClaim<string>> IdentityUserClaims { get; set; }
        public DbSet<ApplicationRole> IdentityUserRoles { get; set; }
        //public DbSet<UserLogin> IdentityUserLogin { get; set; }
        //public DbSet<IdentityRole> IdentityRoles { get; set; }
        //public DbSet<UserDto> Users { get; set; }
        public DbSet<ApplicationUser> IdManagementUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}