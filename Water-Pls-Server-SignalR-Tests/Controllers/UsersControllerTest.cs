using System;
using Xunit;
using SignalRTest;
using SignalRTest.Domain.Dto;
using Moq;
using SignalRTest.DataAccess;
using SignalRTest.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Water_Pls_Server_SignalR_Tests 
{
    public class UsersControllerTest
    {
        [Fact]
        public void UsersController_CreateAsyncUser_ReturnsValidDto()
        {
            // Arrange
            UserDto user = new UserDto(1, "sfds");
            var mockRepo = new Mock<WaterDbContext>(new Mock<DbContextOptions<WaterDbContext>>());
            mockRepo.Setup(repo => repo.Add(user));
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = controller.CreateUserAsync(user);

            // Assert
            Assert.Equal(user, result.Value);
            Assert.Equal("api/Users/1", "api/Users/" + user.Id);
        }
    }
}
