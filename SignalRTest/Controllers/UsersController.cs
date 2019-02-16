﻿using System.Linq;
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

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation($"User {User.Identity.Name} has logged out.");
            return Ok($"User {User.Identity.Name} has logged out.");
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var user = new ApplicationUser {UserName = userLoginDto.username, Email = userLoginDto.email};
            var result = await _userManager.CreateAsync(user, userLoginDto.password);

            if (!result.Succeeded)
            {
                return NotFound($"Error Creatting user {userLoginDto.username}");
            }

            _logger.LogInformation($"Created new account for user '{userLoginDto.username}'");

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            _logger.LogInformation($"USERID: {user.Id}    and CODE: {code}");

            var callbackUrl = Url.Link(
                routeName: "verify.email",
                values: new {usersId = user.Id, code = code}
            );

            _logger.LogInformation("MY CALLBACK URL: " + callbackUrl);

            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            var userCreatedInDb = _dbContext.IdManagementUsers.Single(x => x.UserName == userLoginDto.username);

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

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (!result.Succeeded)
            {
                _logger.LogWarning($"Invalid Email verification token for user {usersId}");
                return BadRequest($"Invalid Email verification token for user {usersId}");
            }

            _logger.LogInformation($"Successfully verified email verification token for {usersId}");

            return Ok($"Successfully verified email verification token for {usersId}");
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
            } else if (result.IsLockedOut)
            {
                _logger.LogWarning($"User {userLoginDto.username} has been locked out");
                return Unauthorized($"User {userLoginDto.username} has been locked out");
            }
            else if (result.RequiresTwoFactor)
            {
                _logger.LogWarning($"User {userLoginDto.username} has not set up TwoFactorAuthentication");
                return Unauthorized($"User {userLoginDto.username} has not set up TwoFactorAuthentication");
            }
            else if (result.IsNotAllowed)
            {
                _logger.LogWarning($"User {userLoginDto.username} is not allowed login. Possible issues: Missing email verification.");
                return Unauthorized($"User {userLoginDto.username} is not allowed to login. Possible issues: Missing email verification.");
            }
            else
            {
                _logger.LogWarning($"Incorrect password User {userLoginDto.username}");
                return NotFound($"Incorrect password User {userLoginDto.username}");
            }
        } 
    }
}