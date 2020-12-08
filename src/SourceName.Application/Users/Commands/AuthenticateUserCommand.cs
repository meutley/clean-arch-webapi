using System.Threading;
using System.Threading.Tasks;

using MediatR;

using SourceName.Application.Common.Interfaces;
using SourceName.Domain.Users;

namespace SourceName.Application.Users.Commands
{
    public class AuthenticateUserCommand : IRequest<string>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public class Handler : IRequestHandler<AuthenticateUserCommand, string>
        {
            private readonly IUserAuthenticationService _userAuthenticationService;
            private readonly IUserRepository _repository;

            public Handler(
                IUserAuthenticationService userAuthenticationService,
                IUserRepository repository
            )
            {
                _userAuthenticationService = userAuthenticationService;
                _repository = repository;
            }
            
            public async Task<string> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
            {
                var entity = await _repository.GetByEmail(request.Email, cancellationToken);
                return await Task.FromResult(
                    _userAuthenticationService.GenerateToken(
                        entity,
                        request.Password
                    )
                );
            }
        }
    }
}