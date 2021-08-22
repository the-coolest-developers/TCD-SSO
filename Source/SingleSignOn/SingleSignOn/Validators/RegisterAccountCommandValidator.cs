using FluentValidation;
using FluentValidation.Results;
using SingleSignOn.Commands;

namespace SingleSignOn.DataAccess.Validators
{
    public class RegisterAccountCommandValidator : AbstractValidator<RegisterAccount.RegisterAccountCommand>
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

            RuleFor(a => a.LastName)
                .MaximumLength(10)
                .WithMessage("401")
                .NotNull()
                .NotEmpty()
                .WithMessage("ERROR");
        }

        public override ValidationResult Validate(ValidationContext<RegisterAccount.RegisterAccountCommand> context)
        {
            return base.Validate(context);
        }
    }
}
