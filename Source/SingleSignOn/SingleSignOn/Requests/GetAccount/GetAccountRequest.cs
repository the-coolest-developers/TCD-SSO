using MediatR;
using System;
using WebApiBaseLibrary.Responses;

namespace SingleSignOn.Requests.GetAccount
{
    public class GetAccountRequest : IRequest<Response<GetAccountResponse>>
    {
        public Guid AccountId { get; set; }
    }
}
