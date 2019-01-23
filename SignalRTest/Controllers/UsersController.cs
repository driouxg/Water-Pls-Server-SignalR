﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalRTest.DataAccess;
using SignalRTest.Domain.Dto;

namespace SignalRTest.Controllers
{
    // This changes to uri to localhost:5000/api/Users
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly WaterDbContext _dbContext;

        public UsersController(WaterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public ActionResult<UserDto> CreateUserAsync(UserDto user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var userCreatedInDb = _dbContext.Users.Single(u => u.Username == user.Username);

            return Created($"api/Users/{userCreatedInDb.Id}", userCreatedInDb);
        }

        [HttpGet("hi1")]
        public ActionResult<UserDto> GetUser(UserDto user)
        {
            return _dbContext.Users.Where(b => b.Username == user.Username).ToList();
        }
    }
}