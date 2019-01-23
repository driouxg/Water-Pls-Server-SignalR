using System;
using Xunit;
using SignalRTest;
using SignalRTest.Domain.Dto;
using Moq;
using SignalRTest.DataAccess;
using SignalRTest.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Water_Pls_Server_SignalR_Tests
{
    public class UsersControllerTest
    {
        [Fact]
        public void UsersController_CreateAsyncUser_ReturnsValidDto()
        {
            // Arrange
            UserDto user = new UserDto(1, "sfds");
            var mockRepo = new Mock<WaterDbContext>();
            mockRepo.Setup(repo => repo.Add<UserDto>).Returns(Task.FromResult(GetTestSessions()));
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = controller.CreateUserAsync(user);

            // Assert

        }
    }
}
