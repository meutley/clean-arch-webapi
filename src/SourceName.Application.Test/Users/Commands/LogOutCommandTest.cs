using System.Threading;
using System.Threading.Tasks;

using Moq;
using Xunit;

using SourceName.Application.Common.Interfaces;

namespace SourceName.Application.Users.Commands.Test
{
    public class LogOutCommandTest
    {
        private const string EMAIL = "EMAIL";
        private const string PASSWORD = "PASSWORD";
        
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Mock<IJwtBlacklistService> _jwtBlacklistServiceMock;

        public LogOutCommandTest()
        {
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _jwtBlacklistServiceMock = new Mock<IJwtBlacklistService>();
        }

        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [Theory]
        public async Task Handle_When_CurrentUserService_Token_NullOrWhiteSpace_Should_Not_Call_JwtBlacklistService(string token)
        {
            _currentUserServiceMock.SetupGet(x => x.Token).Returns(token);

            var command = new LogOutCommand();
            var handler = new LogOutCommand.Handler(_currentUserServiceMock.Object, _jwtBlacklistServiceMock.Object);
            await handler.Handle(command, CancellationToken.None);

            _jwtBlacklistServiceMock.Verify(x => x.BlacklistToken(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Call_JwtBlacklistService_Blacklist_Token()
        {
            _currentUserServiceMock.SetupGet(x => x.Token).Returns("TOKEN");

            var command = new LogOutCommand();
            var handler = new LogOutCommand.Handler(_currentUserServiceMock.Object, _jwtBlacklistServiceMock.Object);
            await handler.Handle(command, CancellationToken.None);

            _jwtBlacklistServiceMock.Verify(x => x.BlacklistToken("TOKEN"), Times.Once);
        }
    }
}