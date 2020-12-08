using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using Moq;
using Xunit;

using SourceName.Domain.Users;

namespace SourceName.Application.Users.Queries.Test
{
    public class GetUserByEmailQueryTest
    {
        private const string EMAIL = "EMAIL";
        
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserRepository> _repositoryMock;

        public GetUserByEmailQueryTest()
        {
            _mapperMock = new Mock<IMapper>();
            _repositoryMock = new Mock<IUserRepository>();
        }

        [Fact]
        public async Task Handle_Should_Call_Repository_GetByEmail()
        {
            _repositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new User()));

            var query = new GetUserByEmailQuery(EMAIL);
            var handler = new GetUserByEmailQuery.Handler(_mapperMock.Object, _repositoryMock.Object);
            await handler.Handle(query, CancellationToken.None);

            _repositoryMock.Verify(x => x.GetByEmail(EMAIL, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Map_User_Entity_To_UserDto()
        {
            var entity = new User();
            _repositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(entity));

            var query = new GetUserByEmailQuery(EMAIL);
            var handler = new GetUserByEmailQuery.Handler(_mapperMock.Object, _repositoryMock.Object);
            await handler.Handle(query, CancellationToken.None);

            _mapperMock.Verify(x => x.Map<UserDto>(entity), Times.Once);
        }
    }
}