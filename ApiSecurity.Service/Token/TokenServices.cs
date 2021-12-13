using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using ApiSecurity.Model.User.Entities;
using Microsoft.IdentityModel.Tokens;

namespace ApiSecurity.Service.Token
{
    public class TokenServices : ITokenServices
    {
        public string GenerateToken(LoginEntities User, string ShaSecretKey, double DayExpired)
        {


            JwtSecurityToken token;

            // Leemos el secret_key desde nuestro appseting
            var SecretKey = Encoding.UTF8.GetBytes(ShaSecretKey);
            var key = new SymmetricSecurityKey(SecretKey);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
                        {
                            new Claim("UserId", User.UserId.ToString()),
                            new Claim("RolId", User.RolId.ToString()),
                            new Claim(ClaimTypes.Role, User.RolName)
                        };

            ClaimsIdentity identity = new ClaimsIdentity(claims);


            token = new JwtSecurityToken(null, null, identity.Claims, DateTime.Now, DateTime.Now.AddDays(DayExpired), creds);

            var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenHandler;

        }
    }
}
