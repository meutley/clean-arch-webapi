using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SourceName.Application.Common.Configuration;
using SourceName.Application.Common.Interfaces;

namespace SourceName.Api.Core.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(
            HttpContext context,
            IOptionsSnapshot<AuthenticationConfiguration> authenticationConfiguration,
            IJwtBlacklistService jwtBlacklistService
        )
        {
            var isTokenValid = true;
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token is object)
            {
                isTokenValid = await ValidateToken(
                    context,
                    authenticationConfiguration.Value,
                    jwtBlacklistService,
                    token
                );
            }

            if (isTokenValid)
            {
                await _next(context);
            }
            else
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
            }
        }

        private async Task<bool> ValidateToken(
            HttpContext context,
            AuthenticationConfiguration authenticationConfiguration,
            IJwtBlacklistService jwtBlacklistService,
            string token
        )
        {
            if (await jwtBlacklistService.IsTokenBlacklisted(token))
            {
                return false;
            }
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(authenticationConfiguration.TokenSecret);
            tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = authenticationConfiguration.Issuer,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                },
                out SecurityToken validatedToken
            );

            if (!(validatedToken is JwtSecurityToken))
            {
                return false;
            }

            return true;
        }
    }
}