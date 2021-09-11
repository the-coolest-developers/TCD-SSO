using MediatR;
using WebApiBaseLibrary.Responses;

namespace SingleSignOn.Commands.RegisterAccount
{
    public class RegisterAccountCommand : IRequest<Response<Unit>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
