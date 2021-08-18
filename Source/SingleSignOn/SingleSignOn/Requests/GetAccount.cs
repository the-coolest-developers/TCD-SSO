using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SingleSignOn.DataAccess.Repositories;
using SingleSignOn.Validators;
using WebApiBaseLibrary.Enums;
using WebApiBaseLibrary.Responses;

namespace SingleSignOn.Requests
{
    public class GetAccount
    {
        public class GetAccountRequest : IRequest<Response<GetAccountResponse>>
        {
            public Guid AccountId { get; set; }
        }

        public class GetAccountCommandHandler : IRequestHandler<GetAccountRequest, Response<GetAccountResponse>>
        {
            private readonly IAccountRepository _accountRepository;
            private readonly GetAccountRequestValidator _validator;

            public GetAccountCommandHandler(IAccountRepository accountRepository)
            {
                _accountRepository = accountRepository;
                _validator = new GetAccountRequestValidator();
            }

            public async Task<Response<GetAccountResponse>> Handle(
                GetAccountRequest request,
                CancellationToken cancellationToken)
            {
                var res = _validator.Validate(request);

                var account = await _accountRepository.GetAsync(request.AccountId);

                if (account != null && res.IsValid)
                {
                    var getAccountResponse = new GetAccountResponse
                    {
                        FirstName = account.FirstName,
                        LastName = account.LastName
                    };

                    return new Response<GetAccountResponse>
                    {
                        Result = getAccountResponse,
                        Status = ResponseStatus.Success
                    };
                }

                return new Response<GetAccountResponse>
                {
                    Status = ResponseStatus.Unauthorized
                };
            }
        }

        public class GetAccountResponse
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}