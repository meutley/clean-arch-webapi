using System.Threading;
using System.Threading.Tasks;

using MediatR;

using SourceName.Application.Common.Interfaces;

namespace SourceName.Application.Users.Commands
{
    public class LogOutCommand : IRequest<Unit>
    {
        public class Handler : IRequestHandler<LogOutCommand>
        {
            private readonly ICurrentUserService _currentUserService;
            private readonly IJwtBlacklistService _jwtBlacklistService;

            public Handler(
                ICurrentUserService currentUserService,
                IJwtBlacklistService jwtBlacklistService
            )
            {
                _currentUserService = currentUserService;
                _jwtBlacklistService = jwtBlacklistService;
            }
            
            public async Task<Unit> Handle(LogOutCommand command, CancellationToken cancellationToken)
            {
                var token = _currentUserService.Token;
                if (string.IsNullOrWhiteSpace(token))
                {
                    return Unit.Value;
                }
                
                await _jwtBlacklistService.BlacklistToken(token);
                return Unit.Value;
            }
        }
    }
}