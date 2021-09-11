using FluentValidation;
using SingleSignOn.Requests.GetAccount;

namespace SingleSignOn.Validators
{
    public class GetAccountRequestValidator : AbstractValidator<GetAccountRequest>
    {
        public GetAccountRequestValidator() 
        {
            RuleFor(request => request.AccountId)
                .NotEmpty()
                .NotNull();
        }
    }
}
