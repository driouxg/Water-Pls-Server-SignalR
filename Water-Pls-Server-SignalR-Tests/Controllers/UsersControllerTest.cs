using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Xunit;
using SignalRTest;
using SignalRTest.Domain.Dto;
using Moq;
using SignalRTest.DataAccess;
using SignalRTest.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SignalRTest.Domain;

namespace Water_Pls_Server_SignalR_Tests 
{
    public class UsersControllerTest
    {
        //Mock<WaterDbContext> mockRepo = new Mock<WaterDbContext>(new Mock<DbContextOptions<WaterDbContext>>());
        Mock<ILogger<UsersController>> mockLogger = new Mock<ILogger<UsersController>>();
        Mock<SignInManager<ApplicationUser>> mockSignInManager = new Mock<SignInManager<ApplicationUser>>();
        //Mock<UserManager<ApplicationUser>> mockUserManager = MockUserManager<UserRegistrationDto>(new List<UserRegistrationDto>()
        //{
        //    CreateValidUserLoginDto()
        //}
        //);
        Mock<IEmailSender> mockEmailSender = new Mock<IEmailSender>();

        [Fact]
        public void UsersController_Register_SuccessfullyRegistersUser()
        {
            // Arrange
            UserRegistrationDto userRegistrationDto = CreateValidUserLoginDto();
            //var controller = new UsersController(mockRepo.Object, mockSignInManager.Object, mockUserManager.Object, mockEmailSender.Object, mockLogger.Object);
            //mockUserManager = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>());
            //mockUserManager.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
            //mockUserManager.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());

            //mockUserManager = MockUserManager(ApplicationUser);

            // Act
            //var result = controller.Register(userRegistrationDto);

            // Assert
            //Assert.IsType<OkObjectResult>(result);
        }

        //public async Task<int> CreateUser(ApplicationUser user, string password) => (await _userManager.CreateAsync(user, password)).Succeeded ? user.Id : -1;

        public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.GenerateChangeEmailTokenAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync("mysupersecrettoken");

            return mgr;
        }
        private UserDto CreateValidUserDto()
        {
            return new UserDto
            {
                email = "test@biscuit.com",
                firstName = "Zelda",
                lastName = "Peach",
                Username = "zepe1",
                addressDto = new AddressDto
                {
                    cityName = "Harold",
                    route = "8th Avenue",
                    stateName = "Arizona",
                    streetName = "Litchfield",
                    streetNumber = 15464,
                    zipcode = 85394
                },
                geoCoordinatesDto = new GeoCoordinatesDto()
                {
                    latitude = 12.234f,
                    longitude = 3124.23423f
                }
            };
        }

        public static UserRegistrationDto CreateValidUserLoginDto()
        {
            return new UserRegistrationDto
            {
                username = "biscuit",
                email = "hellothere@tasty.com",
                password = "mySuperSecretPassword!3",
            };
        }
    }
}
