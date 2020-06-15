using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using MediatR;

using SourceName.Domain.Users;

namespace SourceName.Application.Users.Queries
{
    public class GetUserByEmailQuery : IRequest<UserDto>
    {
        public string Email { get; set; }

        public GetUserByEmailQuery(string email) => Email = email;

        public class Handler : IRequestHandler<GetUserByEmailQuery, UserDto>
        {
            private readonly IMapper _mapper;
            private readonly IUserRepository _repository;

            public Handler(
                IMapper mapper,
                IUserRepository repository
            )
            {
                _mapper = mapper;
                _repository = repository;
            }
            
            public async Task<UserDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
            {
                return _mapper.Map<UserDto>(
                    await _repository.GetByEmail(request.Email, cancellationToken)
                );
            }
        }
    }
}