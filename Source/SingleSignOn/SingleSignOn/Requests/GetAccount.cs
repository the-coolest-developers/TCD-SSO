using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SingleSignOn.DataAccess.Repositories;
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

            public GetAccountCommandHandler(IAccountRepository accountRepository)
            {
                _accountRepository = accountRepository;
            }

            public Task<Response<GetAccountResponse>> Handle(
                GetAccountRequest request,
                CancellationToken cancellationToken)
            {
                return Task.FromResult(new Response<GetAccountResponse>
                {
                    Status = ResponseStatus.Accepted
                });
            }
        }

        public class GetAccountResponse
        {
            public string Token { get; set; }
        }
    }
}