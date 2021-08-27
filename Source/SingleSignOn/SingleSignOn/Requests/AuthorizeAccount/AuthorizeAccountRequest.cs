using MediatR;
using WebApiBaseLibrary.Responses;

namespace SingleSignOn.Requests.AuthorizeAccount
{
    public class AuthorizeAccountRequest : IRequest<Response<AuthorizeAccountResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
