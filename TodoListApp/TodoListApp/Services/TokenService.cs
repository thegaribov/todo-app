using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoListApp.Persistance.Entities;
using TodoListApp.Persistance.Entities.Enums;

namespace TodoListApp.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var key = _configuration.GetSection("JwtConfigs:Key").Value;
            var keyAsBytes = Encoding.UTF8.GetBytes(key);
            var validDays = _configuration.GetSection("JwtConfigs").GetValue<int>("ValidDays");

            var claims = new List<Claim>()
            {
                new Claim("Id", user.Id.ToString())
            };

            if (user.Role == Role.Admin)
            {
                claims.Add(new Claim(ClaimTypes.Role, Role.Admin.ToString()));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(validDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyAsBytes), SecurityAlgorithms.HmacSha256)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }
    }
}
