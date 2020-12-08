using System;

using Microsoft.Extensions.Options;
using Moq;
using Xunit;

using SourceName.Application.Common.Configuration;
using SourceName.Application.Common.Interfaces;
using SourceName.Domain.Users;

namespace SourceName.Infrastructure.Services.Test
{
    public class UserAuthenticationServiceTest
    {
        private const string PASSWORD = "PASSWORD";
        private const string TOKEN = "TOKEN";
        private static readonly TimeSpan TOKEN_LIFETIME = TimeSpan.FromSeconds(1800);
        private const string USER_ID = "USER_ID";

        private readonly AuthenticationConfiguration _authenticationConfiguration;
        private readonly Mock<IUserPasswordService> _userPasswordServiceMock;

        private readonly UserAuthenticationService _target;

        public UserAuthenticationServiceTest()
        {
            _authenticationConfiguration = new AuthenticationConfiguration
            {
                Audience = "AUDIENCE",
                Issuer = "ISSUER",
                TokenSecret = "GENERATETOKENSECRET",
                TokenLifetimeInSeconds = 1800
            };

            var authenticationConfigurationMock = new Mock<IOptionsSnapshot<AuthenticationConfiguration>>();
            authenticationConfigurationMock.Setup(x => x.Value).Returns(_authenticationConfiguration);

            _userPasswordServiceMock = new Mock<IUserPasswordService>();

            _target = new UserAuthenticationService(
                authenticationConfigurationMock.Object,
                _userPasswordServiceMock.Object
            );
        }

        [Fact]
        public void GenerateToken_When_User_Is_Inactive_Should_Throw_UnauthorizedAccessException()
        {
            Assert.Throws<UnauthorizedAccessException>(() =>
                _target.GenerateToken(new User { IsActive = false }, PASSWORD)
            );
        }

        [Fact]
        public void GenerateToken_When_UserPasswordService_ValidateHash_Is_False_Should_Throw_UnauthorizedAccessException()
        {
            _userPasswordServiceMock.Setup(x => x.ValidateHash(
                    It.IsAny<string>(),
                    It.IsAny<byte[]>(),
                    It.IsAny<byte[]>()
                )
            ).Returns(false);
            
            Assert.Throws<UnauthorizedAccessException>(() =>
                _target.GenerateToken(new User { IsActive = true }, PASSWORD)
            );
        }

        [Fact]
        public void GenerateToken_When_Password_Is_Valid_Should_Return_Token()
        {
            _userPasswordServiceMock.Setup(x => x.ValidateHash(
                    It.IsAny<string>(),
                    It.IsAny<byte[]>(),
                    It.IsAny<byte[]>()
                )
            ).Returns(true);

            var actual = _target.GenerateToken(new User { Id = USER_ID, IsActive = true }, PASSWORD);

            Assert.False(string.IsNullOrWhiteSpace(actual));
        }
    }
}