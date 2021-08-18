using FluentValidation;
using SingleSignOn.Requests;

namespace SingleSignOn.Validators
{
    public class GetAccountRequestValidator : AbstractValidator<GetAccount.GetAccountRequest>
    {
        public GetAccountRequestValidator() 
        {
            RuleFor(request => request.AccountId)
                .NotEmpty()
                .NotNull();
        }
    }
}
