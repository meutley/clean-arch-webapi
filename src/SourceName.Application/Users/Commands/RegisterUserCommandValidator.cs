using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using SourceName.Domain.Users;

namespace SourceName.Application.Users.Commands
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public RegisterUserCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            
            RuleFor(x => x.Email)
                .EmailAddress()
                .MaximumLength(320)
                .MustAsync(BeUniqueEmail)
                    .WithMessage("Email address is already in use")
                .NotEmpty();

            RuleFor(x => x.DisplayName)
                .MinimumLength(3)
                .MaximumLength(50)
                .NotEmpty();

            RuleFor(x => x.Password)
                .MinimumLength(6)
                .MaximumLength(25)
                .NotEmpty();
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return (await _userRepository.GetByEmail(email, cancellationToken)) is null;
        }
    }
}