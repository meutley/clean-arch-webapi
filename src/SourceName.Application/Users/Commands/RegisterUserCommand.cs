using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using MediatR;

using SourceName.Application.Common.Interfaces;
using SourceName.Domain.Users;

namespace SourceName.Application.Users.Commands
{
    public class RegisterUserCommand : IRequest
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }

        public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
        {
            private readonly IMapper _mapper;
            private readonly IUserPasswordService _passwordService;
            private readonly IUserRepository _repository;
            
            public RegisterUserCommandHandler(
                IMapper mapper,
                IUserPasswordService passwordService,
                IUserRepository repository
            )
            {
                _mapper = mapper;
                _passwordService = passwordService;
                _repository = repository;
            }
            
            public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            {
                byte[] passwordHash;
                byte[] passwordSalt;
                _passwordService.CreateHash(request.Password, out passwordHash, out passwordSalt);
                var entity = new User
                {
                    Email = request.Email,
                    DisplayName = request.DisplayName,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };

                await _repository.Insert(entity, cancellationToken);
                return Unit.Value;
            }
        }
    }
}