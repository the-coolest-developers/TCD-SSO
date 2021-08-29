using MediatR;
using SingleSignOn.DataAccess.Repositories;
using System.Threading;
using System.Threading.Tasks;
using WebApiBaseLibrary.Enums;
using WebApiBaseLibrary.Responses;

namespace SingleSignOn.Requests.GetAccount
{
    public class GetAccountCommandHandler : IRequestHandler<GetAccountRequest, Response<GetAccountResponse>>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Response<GetAccountResponse>> Handle(
            GetAccountRequest request,
            CancellationToken cancellationToken)
        {

            var account = await _accountRepository.GetAsync(request.AccountId);

            if (account != null)
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
}
