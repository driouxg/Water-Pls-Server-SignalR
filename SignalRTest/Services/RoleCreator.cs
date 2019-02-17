using Microsoft.AspNetCore.Identity;
using SignalRTest.DataAccess;
using SignalRTest.Domain;
using SignalRTest.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTest.Services
{
    public class RoleCreator
    {
        public static async Task Initialize(WaterDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationUser> roleManager)
        {
            dbContext.Database.EnsureCreated();

            String adminId1 = "";
            String adminId2 = "";

            string role1 = "Admin";
            string desc1 = "This is the description for the admin";

            string role2 = "Member";
            string desc2 = "This is the description for the member";

            string password = "P@$$w0rd";

            if (await roleManager.FindByNameAsync(role1) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(role1, desc1, DateTime.Now));
            }
            if (await roleManager.FindByNameAsync(role2) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(role2, desc2, DateTime.Now));
            }
        }
    }
}
