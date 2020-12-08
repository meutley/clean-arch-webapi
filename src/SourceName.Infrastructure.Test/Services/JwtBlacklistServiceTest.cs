using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;
using Moq;
using Xunit;

using SourceName.Application.Common.Configuration;
using SourceName.Application.Common.Interfaces;

namespace SourceName.Infrastructure.Services.Test
{
    public class JwtBlacklistServiceTest
    {
        private const string TOKEN = "TOKEN";
        private static readonly TimeSpan TOKEN_LIFETIME = TimeSpan.FromSeconds(1800);
        
        private readonly AuthenticationConfiguration _authenticationConfiguration;
        private readonly Mock<IApplicationCache> _applicationCacheMock;

        private readonly JwtBlacklistService _target;

        public JwtBlacklistServiceTest()
        {
            _authenticationConfiguration = new AuthenticationConfiguration
            {
                Audience = "AUDIENCE",
                Issuer = "ISSUER",
                TokenSecret = "SECRET",
                TokenLifetimeInSeconds = 1800
            };

            var authenticationConfigurationMock = new Mock<IOptionsSnapshot<AuthenticationConfiguration>>();
            authenticationConfigurationMock.Setup(x => x.Value).Returns(_authenticationConfiguration);

            _applicationCacheMock = new Mock<IApplicationCache>();

            _target = new JwtBlacklistService(
                authenticationConfigurationMock.Object,
                _applicationCacheMock.Object
            );
        }

        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [Theory]
        public void BlacklistToken_When_Token_Is_NullOrWhiteSpace_Throws_ArgumentNullException(string token)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _target.BlacklistToken(token));
        }

        [Fact]
        public async Task BlacklistToken_Should_Call_Cache_AddOrUpdate()
        {
            await _target.BlacklistToken(TOKEN);

            _applicationCacheMock.Verify(x => x.AddOrUpdate(
                $"JWT_BLACKLIST_{TOKEN}",
                TOKEN,
                TOKEN_LIFETIME
            ), Times.Once);
        }

        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [Theory]
        public async Task IsTokenBlacklisted_When_Token_Is_NullOrWhiteSpace_Should_Return_False(string token)
        {
            var actual = await _target.IsTokenBlacklisted(token);

            Assert.False(actual);
        }

        [Fact]
        public async Task IsTokenBlacklisted_Should_Call_Cache_Get()
        {
            await _target.IsTokenBlacklisted(TOKEN);

            _applicationCacheMock.Verify(x => x.Get($"JWT_BLACKLIST_{TOKEN}"), Times.Once);
        }
    }
}