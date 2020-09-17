using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Something.Security
{
    public class SomethingUserManager : ISomethingUserManager
    {
        public ClaimsPrincipal GetUserPrinciple()
        {
            var customClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Example"),
                new Claim(ClaimTypes.Email, "example@mail.com"),
                new Claim(ClaimTypes.Role, "Admin"),
            };

            var customIdentity = new ClaimsIdentity(customClaims, "Custom Identity");

            var userPrincipal = new ClaimsPrincipal(new[] { customIdentity });
            return userPrincipal;
        }

        public string GetUserToken()
        {
            var customClaims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, "Example"),
                new Claim(ClaimTypes.Email, "example@mail.com"),
                new Claim(ClaimTypes.Role, "Admin"),
            };

            var secretBytes = Encoding.UTF8.GetBytes(JwtConstants.Secret);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;
            var signingCredentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                JwtConstants.Issuer,
                JwtConstants.Audience,
                customClaims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(1),
                signingCredentials
                );

            string tokenJson = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenJson;
        }
    }
}
