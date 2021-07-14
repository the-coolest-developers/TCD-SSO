using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SingleSignOn.DataAccess.Repositories;
using WebApiBaseLibrary.Enums;
using WebApiBaseLibrary.Responses;

namespace SingleSignOn.Commands
{
    public class RegisterAccount
    {
        public class RegisterAccountCommand : IRequest<Response<Unit>>
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string FullName { get; set; }
        }

        public class RegisterAccountCommandHandler : IRequestHandler<RegisterAccountCommand, Response<Unit>>
        {
            private readonly IAccountRepository _accountRepository;

            public RegisterAccountCommandHandler(IAccountRepository accountRepository)
            {
                _accountRepository = accountRepository;
            }

            public Task<Response<Unit>> Handle(
                RegisterAccountCommand request,
                CancellationToken cancellationToken)
            {
                return Task.FromResult(new Response<Unit>
                {
                    Status = ResponseStatus.Accepted
                });
            }
        }
    }
}