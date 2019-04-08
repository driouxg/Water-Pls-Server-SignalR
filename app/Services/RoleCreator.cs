using Microsoft.AspNetCore.Identity;
using SignalRTest.Domain;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SignalRTest.DataAccess;
using SignalRTest.Domain.Entity;

namespace SignalRTest.Services
{
    public class RoleCreator
    {
        private readonly WaterDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;

        public RoleCreator(WaterDbContext dbContext, UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task Initialize()
        {
            _dbContext.Database.EnsureCreated();

            string name1 = "admin";
            string role1 = "Administrator";
            string desc1 = "This is the description for the admin";

            await CreateRoleIfNonExistent(role1, desc1);

            await CreateItems(name1, role1, name1);
        }

        public async Task CreateRoleIfNonExistent(string role, string description)
        {
            if (await _roleManager.FindByNameAsync(role) == null)
            {
                await _roleManager.CreateAsync(new ApplicationRole(role, description, DateTime.Now));
            }
        }

        public async Task CreateItems(string email, string role, string username)
        {
            if (await _userManager.FindByNameAsync(email) == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = email
                };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.AddPasswordAsync(user, _configuration["ADMIN_PASSWORD"]);
                    await _userManager.SetTwoFactorEnabledAsync(user, true);
                    await _userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
