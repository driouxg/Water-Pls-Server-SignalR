using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignalRTest.DataAccess;
using SignalRTest.Domain.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using SignalRTest.Domain;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Encodings.Web;

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
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public UsersController(WaterDbContext dbContext, 
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            ILogger<UsersController> logger)
        {
            _dbContext = dbContext;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
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

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _signInManager.SignOutAsync();
            _logger.LogInformation($"User {userLoginDto.username} has logged out.");
            return Ok();
        }

        [HttpPost("logoutHttpContext")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserLoginDto userLoginDto, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var user = new ApplicationUser {UserName = userLoginDto.username, Email = userLoginDto.email};
            var result = await _userManager.CreateAsync(user, userLoginDto.password);

            if (result.Succeeded)
            {
                _logger.LogInformation($"Created new account for user '{userLoginDto.username}'");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                //var callbackUrl = Url.Page("/Account/ConfirmEmail",
                //    pageHandler: null,
                //    values: new {userId = user.Id, code = code},
                //    protocol: Request.Scheme);

                _logger.LogInformation($"USERID: {user.Id}    and CODE: {code}");

                var callbackUrl = Url.Link(
                    routeName: "yoyo",
                    values: new {usersId = user.Id, code = code}
                );

                _logger.LogInformation("MY CALLBACK URL: " + callbackUrl);

                await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                //await _emailSender.SendEmailAsync(user.Email, "Confirm Your Email", "Hello");

                //return LocalRedirect(returnUrl);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("authenticate")]
        [Route("authenticate/{usersId}/{code}", Name = "yoyo")]
        public async Task<IActionResult> Authenticate(string usersId, string code)
        {
            _logger.LogInformation($"REACHED!!!!    with {usersId}");

            var user = await _userManager.FindByIdAsync(usersId);
            code = code.Replace("%2f", "/").Replace("%2F", "/");

            if (user == null)
            {
                _logger.LogWarning($"Could not find user by id {usersId}");
                return BadRequest($"Could not find user by id {usersId}");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (!result.Succeeded)
            {
                _logger.LogWarning($"Invalid Email verification token for user {usersId}");
                return BadRequest($"Invalid Email verification token for user {usersId}");
            }

            _logger.LogInformation($"Successfully verified email verification token for {usersId}");

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var result = await _signInManager.PasswordSignInAsync(userLoginDto.username, userLoginDto.password, userLoginDto.rememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                _logger.LogInformation($"User {userLoginDto.username} logged in.");
            } else if (result.IsLockedOut)
            {
                _logger.LogWarning($"User {userLoginDto.username} has been locked out");
            }

            return Ok();
        } 

        [HttpPost("loginCookie")]
        public async Task<IActionResult> LoginCookie(UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var claims = new List<Claim>
            {
                new Claim("user", userLoginDto.username),
                new Claim("role", "Member")
            };

            await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));

            return Ok();
        }
    }
}