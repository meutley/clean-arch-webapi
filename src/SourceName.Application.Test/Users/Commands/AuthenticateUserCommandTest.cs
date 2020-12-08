using System.Threading;
using System.Threading.Tasks;

using Moq;
using Xunit;

using SourceName.Application.Common.Interfaces;
using SourceName.Domain.Users;

namespace SourceName.Application.Users.Commands.Test
{
    public class AuthenticateUserCommandTest
    {
        private const string EMAIL = "EMAIL";
        private const string PASSWORD = "PASSWORD";
        
        private readonly Mock<IUserAuthenticationService> _userAuthenticationServiceMock;
        private readonly Mock<IUserRepository> _repositoryMock;

        public AuthenticateUserCommandTest()
        {
            _userAuthenticationServiceMock = new Mock<IUserAuthenticationService>();
            _repositoryMock = new Mock<IUserRepository>();
        }

        [Fact]
        public async Task Handle_Should_Call_Repository_GetByEmail()
        {
            _repositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new User()));

            var command = new AuthenticateUserCommand { Email = EMAIL, Password = PASSWORD };
            var handler = new AuthenticateUserCommand.Handler(_userAuthenticationServiceMock.Object, _repositoryMock.Object);
            await handler.Handle(command, CancellationToken.None);

            _repositoryMock.Verify(x => x.GetByEmail(EMAIL, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Call_UserAuthenticationService_GenerateToken()
        {
            var entity = new User();
            _repositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(entity));

            var command = new AuthenticateUserCommand { Email = EMAIL, Password = PASSWORD };
            var handler = new AuthenticateUserCommand.Handler(_userAuthenticationServiceMock.Object, _repositoryMock.Object);
            await handler.Handle(command, CancellationToken.None);

            _userAuthenticationServiceMock.Verify(x => x.GenerateToken(entity, PASSWORD), Times.Once);
        }
    }
}