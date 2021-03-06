using FluentValidation;
using SingleSignOn.Commands.RegisterAccount;

namespace SingleSignOn.Validators
{
    public class RegisterAccountCommandValidator : AbstractValidator<RegisterAccountCommand>
    {
        public RegisterAccountCommandValidator()
        {
            RuleFor(command => command.Email)
                .EmailAddress()
                .MaximumLength(50)
                .NotEmpty()
                .NotNull();

            RuleFor(command => command.Password)
                .MaximumLength(50)
                .NotNull()
                .NotEmpty();

            RuleFor(command => command.FirstName)
                .MaximumLength(50)
                .NotNull()
                .NotEmpty();

            RuleFor(command => command.LastName)
                .MaximumLength(50)
                .NotEmpty()
                .NotNull();
        }
    }
}
