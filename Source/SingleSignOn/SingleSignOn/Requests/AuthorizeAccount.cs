using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebApiBaseLibrary.Enums;
using WebApiBaseLibrary.Responses;

namespace SingleSignOn.Requests
{
    public class AuthorizeAccount
    {
        public class AuthorizeAccountRequest : IRequest<Response<AuthorizeAccountResponse>>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class AuthorizeAccountCommandHandler :
            IRequestHandler<AuthorizeAccountRequest, Response<AuthorizeAccountResponse>>
        {
            public Task<Response<AuthorizeAccountResponse>> Handle(
                AuthorizeAccountRequest request,
                CancellationToken cancellationToken)
            {
                return Task.FromResult(new Response<AuthorizeAccountResponse>
                {
                    Status = ResponseStatus.Accepted
                });
            }
        }

        public class AuthorizeAccountResponse
        {
            public string Token { get; set; }
        }
    }
}