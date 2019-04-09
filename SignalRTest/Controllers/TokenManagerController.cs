using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalRTest.Domain.Dto;
using SignalRTest.Services;

namespace SignalRTest.Controllers
{
    [ApiController]
    [Route("api/token")]
    public class TokenManagerController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ITokenManagerService _tokenManagerService;

        public TokenManagerController(ILogger<TokenManagerController> logger, ITokenManagerService tokenManagerService)
        {
            _logger = logger;
            _tokenManagerService = tokenManagerService;
        }

        [HttpPost("generate")]
        public async Task<AuthenticationTokenDto> GenerateToken(UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Invalid login credentials for user {userLoginDto.username}");
                return new AuthenticationTokenDto
                {
                    Token = "Invalid Login Credentials"
                };
            }

            var token = await _tokenManagerService.GenerateToken(userLoginDto.username, userLoginDto.password);

            _logger.LogInformation($"Generated token: {token} for user {userLoginDto.username}");

            return new AuthenticationTokenDto
            {
                Token = token
            };
        }

        [HttpPost("refresh")]
        public AuthenticationTokenDto RefreshToken(AuthenticationTokenDto tokenDto)
        {
            if (!ModelState.IsValid)
            {
                return new AuthenticationTokenDto
                {
                    Token = "Invalid token"
                };
            }
            var token = _tokenManagerService.RefreshToken(tokenDto.Token);
            return new AuthenticationTokenDto
            {
                Token = token
            };
        }
    }
}
