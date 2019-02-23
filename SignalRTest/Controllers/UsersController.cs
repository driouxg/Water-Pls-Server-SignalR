using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignalRTest.DataAccess;
using SignalRTest.Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using SignalRTest.Domain;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using SignalRTest.Domain.Entity;
using SignalRTest.Services;

namespace SignalRTest.Controllers
{
    // This changes to uri to localhost:5000/api/Users
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ITokenAuthenticationService _tokenAuthenticationService;
        private readonly WaterDbContext _dbContext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public UsersController(ITokenAuthenticationService tokenAuthenticationService, 
            WaterDbContext dbContext,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IEmailSender emailSender,
            ILogger<UsersController> logger)
        {
            _tokenAuthenticationService = tokenAuthenticationService;
            _dbContext = dbContext;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation($"User {User.Identity.Name} has logged out.");
            return Ok($"User {User.Identity.Name} has logged out.");
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegistrationDto userRegistrationDto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            
            var user = new ApplicationUser {UserName = userRegistrationDto.username, Email = userRegistrationDto.email};
            var result = await _userManager.CreateAsync(user, userRegistrationDto.password);

            if (!result.Succeeded)
            {
                return NotFound($"Error Creating user {userRegistrationDto.username}, user may already exist!");
            }

            _logger.LogInformation($"Created new account for user '{userRegistrationDto.username}'");

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            _logger.LogInformation($"USERID: {user.Id}    and CODE: {code}");

            var callbackUrl = Url.Link(
                routeName: "verify.email",
                values: new {usersId = user.Id, code = code}
            );

            _logger.LogInformation("MY CALLBACK URL: " + callbackUrl);

            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            var userCreatedInDb = _dbContext.IdManagementUsers.Single(x => x.UserName == userRegistrationDto.username);

            return Created($"api/Users/{userCreatedInDb.Id}", $"Created user {userCreatedInDb.Id}");
        }

        [HttpGet("verifyemail")]
        [Route("verifyemail/{usersId}/{code}", Name = "verify.email")]
        public async Task<IActionResult> VerifyEmail(string usersId, string code)
        {
            var user = await _userManager.FindByIdAsync(usersId);
            code = code.Replace("%2f", "/").Replace("%2F", "/");

            if (user == null)
            {
                _logger.LogWarning($"Could not find user by id {usersId}");
                return BadRequest($"Could not find user by id {usersId}");
            }

            await ApplyMemberRoleToUser(user);

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (!result.Succeeded)
            {
                _logger.LogWarning($"Invalid Email verification token for user {usersId}");
                return BadRequest($"Invalid Email verification token for user {usersId}");
            }

            _logger.LogInformation($"Successfully verified email verification token for {usersId}");

            return Ok($"Successfully verified email verification token for {usersId}");
        }

        [HttpPost("get-authentication-token")]
        public async Task<AuthenticationTokenDto> GetAuthenticationToken(UserLoginDto userLoginDto)
        {
            _logger.LogInformation($"REACHED!");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Invalid login credentials for user {userLoginDto.username}");
                return new AuthenticationTokenDto
                {
                    Token = "Invalid Login Credentials"
                };
            }

            _logger.LogInformation($"About to generate token for user {userLoginDto.username}");

            var token = await _tokenAuthenticationService.Authenticate(userLoginDto.username, userLoginDto.password);

            _logger.LogInformation($"Generated token: {token} for user {userLoginDto.username}");

            return new AuthenticationTokenDto
            {
                Token = token
            };
        }

        public async Task ApplyMemberRoleToUser(ApplicationUser user)
        {
            string role = "Member";

            if (await _roleManager.FindByNameAsync(role) != null)
            {
                await _userManager.AddToRoleAsync(user, role);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Clear the existing external cookie to ensure a clean login process
            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var result = await _signInManager.PasswordSignInAsync(userLoginDto.username, userLoginDto.password, userLoginDto.rememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                _logger.LogInformation($"User {userLoginDto.username} logged in.");
                return Ok($"User {userLoginDto.username} logged in.");
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning($"User {userLoginDto.username} has been locked out");
                return Unauthorized($"User {userLoginDto.username} has been locked out");
            }
            if (result.RequiresTwoFactor)
            {
                _logger.LogWarning($"User {userLoginDto.username} has not set up TwoFactorAuthentication");
                return Unauthorized($"User {userLoginDto.username} has not set up TwoFactorAuthentication");
            }
            if (result.IsNotAllowed)
            {
                _logger.LogWarning($"User {userLoginDto.username} is not allowed login. Possible issues: Missing email verification.");
                return Unauthorized($"User {userLoginDto.username} is not allowed to login. Possible issues: Missing email verification.");
            }
            
            _logger.LogWarning($"Incorrect password User {userLoginDto.username}");
            return NotFound($"Incorrect password User {userLoginDto.username}");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("banuser/{userId}")]
        public async Task<IActionResult> BanUser()
        {
            // Need to figure out a way to ban users
            //_userManager.RemoveClaimAsync();
            return Ok();
        }
    }
}