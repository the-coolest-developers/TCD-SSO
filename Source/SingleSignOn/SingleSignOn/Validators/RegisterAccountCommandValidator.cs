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
                .NotNull()
                .WithMessage("Email is invalid");
            
            RuleFor(command => command.Password)
                .MaximumLength(50)
                .NotNull()
                .NotEmpty()
                .WithMessage("Password is invalid"); 

            RuleFor(command => command.FirstName)
                .MaximumLength(50)
                .NotNull()
                .NotEmpty()
                .WithMessage("First Name is invalid"); 

            RuleFor(command => command.LastName)
                .MaximumLength(50)
                .NotEmpty()
                .NotNull()
                .WithMessage("Last Name is invalid");
        }
    }
}
