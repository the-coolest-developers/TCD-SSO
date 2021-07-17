using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SingleSignOn.DataAccess.Entities;
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
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class
            RegisterAccountCommandHandler :
                IRequestHandler<RegisterAccountCommand, Response<Unit>>
        {
            private readonly IAccountRepository _accountRepository;

            public RegisterAccountCommandHandler(IAccountRepository accountRepository)
            {
                _accountRepository = accountRepository;
            }

            public async Task<Response<Unit>> Handle(
                RegisterAccountCommand request,
                CancellationToken cancellationToken)
            {
                if (await _accountRepository.ExistsWithEmailAsync(request.Email))
                {
                    return new Response<Unit>
                    {
                        Status = ResponseStatus.Unauthorized
                    };
                }

                var account = new Account
                {
                    Id = new Guid(),
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName
                };

                await _accountRepository.CreateAsync(account);
                await _accountRepository.SaveChangesAsync();

                return new Response<Unit>
                {
                    Status = ResponseStatus.Created
                };
            }
        }
    }
}