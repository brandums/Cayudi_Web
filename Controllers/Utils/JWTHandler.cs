using EPlatformWebApp.Model;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Unicode;

namespace EPlatformWebApp.Controllers.Utils
{
    public class JWTHandler
    {
        string _key;
        private readonly IConfiguration _configuration;
        public JWTHandler(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = _configuration.GetSection("JWT:Key").Value!;
        }

        public string createJWT(User user)
        { 
            List<Claim> claimList = new List<Claim> {
                new Claim("id", user.ID.ToString()),
                new Claim("role", user.RoleID.ToString())                
            };

            var sKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));

            var credentials = new SigningCredentials(sKey, SecurityAlgorithms.HmacSha512Signature);

            var sToken =  new JwtSecurityToken(
                    claims : claimList,
                    expires: DateTime.UtcNow.AddDays(7),
                    signingCredentials : credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(sToken);
        }
    }
}
