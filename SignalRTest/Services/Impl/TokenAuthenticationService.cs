using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SignalRTest.Domain;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SignalRTest.Services.Impl
{
    public class TokenAuthenticationService : ITokenAuthenticationService
    {
        private readonly ILogger<TokenAuthenticationService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenAuthenticationService(ILogger<TokenAuthenticationService> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<string> Authenticate(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return null;
            }

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(user.PasswordHash);

            X509Certificate2 cert = new X509Certificate2("C:\\Users\\drioux.guidry\\Desktop\\certificate\\powershellcert.pfx", "password1234", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet |
                                                                                               X509KeyStorageFlags.PersistKeySet);
            var myBytes = cert.GetPublicKey();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(myBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
