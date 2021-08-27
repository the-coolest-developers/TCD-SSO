using MediatR;
using SingleSignOn.DataAccess.Entities;
using SingleSignOn.DataAccess.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApiBaseLibrary.Authorization.Enums;
using WebApiBaseLibrary.Enums;
using WebApiBaseLibrary.Infrastructure.Generators;
using WebApiBaseLibrary.Responses;

namespace SingleSignOn.Commands.RegisterAccount
{
    public class
            RegisterAccountCommandHandler :
                IRequestHandler<RegisterAccountCommand, Response<Unit>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IHashGenerator _hashGenerator;

        public RegisterAccountCommandHandler(
            IAccountRepository accountRepository,
            IHashGenerator hashGenerator)
        {
            _accountRepository = accountRepository;
            _hashGenerator = hashGenerator;
        }

        public async Task<Response<Unit>> Handle(
            RegisterAccountCommand request,
            CancellationToken cancellationToken)
        {

            if (await _accountRepository.ExistsWithEmailAsync(request.Email))
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
                RoleId = (int)UserRole.User,
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
