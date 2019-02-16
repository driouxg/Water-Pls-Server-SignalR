using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalRTest.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTest.Controllers
{
    //[Authorize("Administrator")]
    [Route("api/[controller]")]
    public class UserRolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public UserRolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger<UserRolesController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        // This is for development purposes only, delete once official DB with a super user has been created
        [HttpGet("createsuperuser")]
        public async Task<IActionResult> CreateSuperUser()
        {
            bool roleAlreadyExists = await _roleManager.RoleExistsAsync("Administrator");

            if (!roleAlreadyExists)
            {
                // Create the role
                var role = new IdentityRole();
                role.Name = "Administrator";
                await _roleManager.CreateAsync(role);

                // Here we create a Admin super user who will maintain the website
                var user = new ApplicationUser();
                user.UserName = "dryox";
                user.Email = "dryox@dryoxapps.com";

                string password = "mySuperDuperSecretPassword19f!";
                IdentityResult chkUser = await _userManager.CreateAsync(user, password);

                // Add default User to Role Administrator
                if (chkUser.Succeeded)
                {
                    var result1 = await _userManager.AddToRoleAsync(user, "Administrator");
                }
            }
            // creating Creating Manager role     
            roleAlreadyExists = await _roleManager.RoleExistsAsync("Manager");
            if (!roleAlreadyExists)
            {
                var role = new IdentityRole();
                role.Name = "Manager";
                await _roleManager.CreateAsync(role);
            }

            // creating Creating Employee role     
            roleAlreadyExists = await _roleManager.RoleExistsAsync("Employee");
            if (!roleAlreadyExists)
            {
                var role = new IdentityRole();
                role.Name = "Employee";
                await _roleManager.CreateAsync(role);
            }

            return Ok();
        }
    }
}
