using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SingleSignOn.DataAccess.Entities;
using SingleSignOn.DataAccess.Repositories;
using SingleSignOn.DataAccess.Validators;
using WebApiBaseLibrary.Authorization.Enums;
using WebApiBaseLibrary.Enums;
using WebApiBaseLibrary.Infrastructure.Generators;
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
            private readonly IHashGenerator _hashGenerator;
            private readonly RegisterAccountCommandValidator _validator;

            public RegisterAccountCommandHandler(
                IAccountRepository accountRepository,
                IHashGenerator hashGenerator)
            {
                _accountRepository = accountRepository;
                _hashGenerator = hashGenerator;
                _validator = new RegisterAccountCommandValidator();
            }

            public async Task<Response<Unit>> Handle(
                RegisterAccountCommand request,
                CancellationToken cancellationToken)
            {
                var res = _validator.Validate(request);

                if (await _accountRepository.ExistsWithEmailAsync(request.Email) || !res.IsValid)
                {
                    return new Response<Unit>
                    {
                        Status = ResponseStatus.Conflict
                    };
                }

                var passwordHash = await _hashGenerator.GenerateSaltedHash(request.Password);

                var account = new Account
                {
                    Id = new Guid(),
                    RoleId = (int) UserRole.User,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PasswordHash = passwordHash
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