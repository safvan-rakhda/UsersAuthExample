using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UsersAuthExample.Services.Interfaces;
using static UsersAuthExample.Data.Constants;

namespace UsersAuthExample.Auth
{
    public static class JwtHandler
    {
        public static Task<string> GanerateToken(GanerateTokenRequest request)
        {
            List<Claim> claims = new()
            {
                new Claim(CustomClaimTypes.UserId, request.UserId.ToString()),
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
                jwtoption.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = (e) =>
                    {
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = async (context) =>
                    {
                        var identity = context.Principal.Identity as ClaimsIdentity;
                        ////valid, set access token as claim
                        if (context.SecurityToken is JwtSecurityToken token)
                        {
                            Claim userIdClaim = token.Claims.SingleOrDefault(x => x.Type == CustomClaimTypes.UserId);

                            IUserService _userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                            if (!int.TryParse(userIdClaim.Value, out int userId))
                            {
                                context.Fail("Invalid Token");
                            }

                            var dbUser = await _userService.GetUserById(userId);

                            if (dbUser != null)
                            {
                                identity?.AddClaim(new Claim(ClaimTypes.NameIdentifier, dbUser.UserId.ToString()));
                                return;
                            }
                        }

                        context.Fail("Invalid Token");
                    }
                };
            };
        }
    }
}
