using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UsersAuthExample.Auth
{
    public static class JwtHandler
    {
        public static Task<string> GanerateToken(GanerateTokenRequest request)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, request.UserId.ToString()),
                new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddDays(1).ToString(), ClaimValueTypes.DateTime)
            };

            foreach (var role in request.Roles)
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKeyString"))
                            , SecurityAlgorithms.HmacSha256),
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow
            };
            var handledToken = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(handledToken));
        }

        public static Action<JwtBearerOptions> GanerateJwtHandler()
        {
            return jwtoption =>
            {
                jwtoption.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKeyString")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            };
        }
    }
}
