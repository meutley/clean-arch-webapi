using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using SourceName.Application.Common.Configuration;
using SourceName.Application.Common.Interfaces;
using SourceName.Domain.Users;

namespace SourceName.Infrastructure.Services
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly AuthenticationConfiguration _authenticationConfiguration;
        private readonly IUserPasswordService _userPasswordService;

        public UserAuthenticationService(
            IOptionsSnapshot<AuthenticationConfiguration> authenticationConfiguration,
            IUserPasswordService userPasswordService
        )
        {
            _authenticationConfiguration = authenticationConfiguration.Value;
            _userPasswordService = userPasswordService;
        }

        public string GenerateToken(User entity, string password)
        {
            if (entity?.IsActive != true)
            {
                throw new UnauthorizedAccessException();
            }

            if (!_userPasswordService.ValidateHash(password, entity.PasswordHash, entity.PasswordSalt))
            {
                throw new UnauthorizedAccessException();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authenticationConfiguration.TokenSecret);

            var allClaims = new List<Claim>();
            allClaims.Add(new Claim(ClaimTypes.NameIdentifier, entity.Id.ToString()));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(allClaims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Audience = _authenticationConfiguration.Audience,
                Issuer = _authenticationConfiguration.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}