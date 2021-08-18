using FluentValidation;
using SingleSignOn.Commands;

namespace SingleSignOn.DataAccess.Validators
{
    public class RegisterAccountCommandValidator : AbstractValidator<RegisterAccount.RegisterAccountCommand>
    {
        public RegisterAccountCommandValidator()
        {
            RuleFor(command => command.Email)
                .MaximumLength(50)
                .EmailAddress()
                .NotEmpty()
                .NotNull();
            
            RuleFor(command => command.FirstName)
                .MaximumLength(50)
                .NotNull()
                .NotEmpty();

            RuleFor(command => command.LastName)
                .MaximumLength(50)
                .NotNull()
                .NotEmpty();

            RuleFor(a => a.LastName)
                .MaximumLength(50)
                .NotNull()
                .NotEmpty();
        }
    }
}
