using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SignalRTest.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SignalRTest.Domain.Dto;

namespace SignalRTest.Services.Impl
{
    public class TokenManagerService : ITokenManagerService
    {
        private readonly ILogger<TokenManagerService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        public TokenManagerService(ILogger<TokenManagerService> logger, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
        }
        public string RefreshToken(string token)
        {
            ClaimsPrincipal principal = GetPrincipalFromExpiredToken(token);
            JwtSecurityToken jsonWebToken = GenerateJwtSecurityToken(principal.Claims);
            return ConvertJwtToString(jsonWebToken);
        }
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = GenerateValidationParameters();
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
        public async Task<string> GenerateToken(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                _logger.LogWarning($"Unable to find user {username} in the Identity Database.");
                return null;
            }
            Claim[] claims = {
                new Claim(ClaimTypes.Name, user.Id)
            };
            JwtSecurityToken jsonWebToken = GenerateJwtSecurityToken(claims);
            return ConvertJwtToString(jsonWebToken);
        }
        public JwtSecurityToken GenerateJwtSecurityToken(IEnumerable<Claim> principal)
        {
            SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SIGNING_KEY"]));
            return new JwtSecurityToken(
                claims: principal,
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(DateTime.Now.AddMinutes(60)).DateTime,
                signingCredentials: new SigningCredentials(SIGNING_KEY, SecurityAlgorithms.HmacSha256)
            );
        }
        TokenValidationParameters GenerateValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SIGNING_KEY"])),
                ValidateLifetime = false
            };
        }

        string ConvertJwtToString(JwtSecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
