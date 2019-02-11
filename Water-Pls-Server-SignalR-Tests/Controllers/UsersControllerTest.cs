using System;
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
        [Fact]
        public void UsersController_CreateAsyncUser_ReturnsValidDto()
        {
            // Arrange
            UserDto user = new UserDto
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
                geoCoordinatesDto = new GeoCoordinatesDto
                {
                    latitude = 123.43534,
                    longitude = 2345.234
                },
            };
            var mockRepo = new Mock<WaterDbContext>(new Mock<DbContextOptions<WaterDbContext>>());
            var mockLogger = new Mock<ILogger<UsersController>>();
            var mockSignInManager = new Mock<SignInManager<ApplicationUser>>();
            var mockUserManager = new Mock<UserManager<ApplicationUser>>();
            var mockEmailSender = new Mock<IEmailSender>();


            mockRepo.Setup(repo => repo.Add(user));
            var controller = new UsersController(mockRepo.Object, mockSignInManager.Object, mockUserManager.Object, mockEmailSender.Object, mockLogger.Object);

            // Act
            var result = controller.CreateUserAsync(user);

            // Assert
            Assert.Equal(user, result.Value);
            Assert.Equal("api/Users/1", "api/Users/" + user.Id);
        }
    }
}
