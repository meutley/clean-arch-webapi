using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using Moq;
using Xunit;

using SourceName.Application.Common.Interfaces;
using SourceName.Domain.Users;

namespace SourceName.Application.Users.Commands.Test
{
    public class RegisterUserCommandTest
    {
        private const string DISPLAY_NAME = "DISPLAY_NAME";
        private const string EMAIL = "EMAIL";
        private const string PASSWORD = "PASSWORD";
        
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserPasswordService> _userPasswordServiceMock;
        private readonly Mock<IUserRepository> _repositoryMock;

        public RegisterUserCommandTest()
        {
            _mapperMock = new Mock<IMapper>();
            _userPasswordServiceMock = new Mock<IUserPasswordService>();
            _repositoryMock = new Mock<IUserRepository>();
        }

        [Fact]
        public async Task Handle_Should_Call_PasswordService_CreateHash()
        {
            var command = new RegisterUserCommand { DisplayName = DISPLAY_NAME, Email = EMAIL, Password = PASSWORD };
            var handler = new RegisterUserCommand.Handler(_mapperMock.Object, _userPasswordServiceMock.Object, _repositoryMock.Object);
            await handler.Handle(command, CancellationToken.None);

            var hash = new byte[1];
            var salt = new byte[1];
            _userPasswordServiceMock.Verify(x =>
                x.CreateHash(
                    PASSWORD,
                    out hash,
                    out salt
                ),
            Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Call_Repository_Insert()
        {
            var command = new RegisterUserCommand { DisplayName = DISPLAY_NAME, Email = EMAIL, Password = PASSWORD };
            var handler = new RegisterUserCommand.Handler(_mapperMock.Object, _userPasswordServiceMock.Object, _repositoryMock.Object);
            await handler.Handle(command, CancellationToken.None);

            _repositoryMock.Verify(x =>
                x.Insert(
                    It.Is<User>(u =>
                        u.Email == EMAIL &&
                        u.DisplayName == DISPLAY_NAME
                    ),
                    CancellationToken.None
                ),
            Times.Once);
        }
    }
}