using FluentValidation;
using SingleSignOn.Requests;

namespace SingleSignOn.Validators
{
    public class AuthorizeAccountRequestValidator : AbstractValidator<AuthorizeAccount.AuthorizeAccountRequest>
    {
        public AuthorizeAccountRequestValidator() 
        {
            RuleFor(request => request.Email)
                .EmailAddress()
                .NotEmpty()
                .NotNull()
                .MaximumLength(50);

            RuleFor(request => request.Password)
                .MaximumLength(50)
                .NotNull()
                .NotEmpty();
        }
    }
}
