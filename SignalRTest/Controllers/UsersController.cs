using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignalRTest.DataAccess;
using SignalRTest.Domain.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using SignalRTest.Domain;
using Microsoft.Extensions.Logging;

namespace SignalRTest.Controllers
{
    // This changes to uri to localhost:5000/api/Users
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly WaterDbContext _dbContext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public UsersController(WaterDbContext dbContext, 
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager,
            ILogger<UsersController> logger)
        {
            _dbContext = dbContext;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost("CreateUserAsync")]
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
        public UserDto GetUser(UserDto user)
        {
            return _dbContext.Users.Single(b => b.Username == user.Username);
        }

        [HttpPost("register")]
        public async ActionResult<UserDto> Login(UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(userLoginDto.username, userLoginDto.password, userLoginDto.rememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                _logger.LogInformation($"User {userLoginDto.username} logged in.");
            } else if (result.IsLockedOut)
            {
                _logger.LogWarning($"User {userLoginDto.username} has been locked out");
            }

            return SignIn();
        } 
    }
}