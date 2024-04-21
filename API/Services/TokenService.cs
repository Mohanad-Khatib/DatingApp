using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));  // should have a key
        }
        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>(){
                new Claim(JwtRegisteredClaimNames.NameId, user.Username)  // save the claims
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature); // to sign the token

            var tokenDescriptor = new SecurityTokenDescriptor{   
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}