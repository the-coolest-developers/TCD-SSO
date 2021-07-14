using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
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