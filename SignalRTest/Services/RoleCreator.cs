using Microsoft.AspNetCore.Identity;
using SignalRTest.DataAccess;
using SignalRTest.Domain;
using SignalRTest.Domain.Entity;
using System;
using System.Threading.Tasks;

namespace SignalRTest.Services
{
    public class RoleCreator
    {
        private readonly WaterDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        string password = "P@$$w0rd";

        public RoleCreator(WaterDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Initialize()
        {
            _dbContext.Database.EnsureCreated();

            string name1 = "aa@aa.aa";
            string role1 = "Administrator";
            string desc1 = "This is the description for the admin";

            string name2 = "bb@bb.bb";
            string role2 = "Member";
            string desc2 = "This is the description for the member";

            string name3 = "cc@cc.cc";
            string role3 = "Manager";
            string desc3 = "This is the description for the manager";

            await CreateRoleIfNonExistent(role1, desc1);
            await CreateRoleIfNonExistent(role2, desc2);
            await CreateRoleIfNonExistent(role3, desc3);

            await CreateItems(name1, role1, name1);
            await CreateItems(name2, role2, name2);
            await CreateItems(name3, role3, name3);
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
                    await _userManager.AddPasswordAsync(user, password);
                    await _userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
