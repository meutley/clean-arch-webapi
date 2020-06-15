using FluentValidation;

namespace SourceName.Application.Users.Commands
{
    public class AuthenticateUserCommandValidator : AbstractValidator<AuthenticateUserCommand>
    {
        public AuthenticateUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .MaximumLength(320)
                .NotEmpty();

            RuleFor(x => x.Password)
                .MinimumLength(6)
                .MaximumLength(25)
                .NotEmpty();
        }
    }
}