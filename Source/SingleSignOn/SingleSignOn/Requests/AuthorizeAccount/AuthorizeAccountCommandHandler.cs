using MediatR;
using SingleSignOn.DataAccess.Repositories;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using WebApiBaseLibrary.Authorization.Constants;
using WebApiBaseLibrary.Authorization.Enums;
using WebApiBaseLibrary.Authorization.Generators;
using WebApiBaseLibrary.Enums;
using WebApiBaseLibrary.Infrastructure.Generators;
using WebApiBaseLibrary.Responses;

namespace SingleSignOn.Requests.AuthorizeAccount
{
    public class AuthorizeAccountCommandHandler :
            IRequestHandler<AuthorizeAccountRequest, Response<AuthorizeAccountResponse>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IHashGenerator _hashGenerator;
        private readonly IJwtGenerator _jwtGenerator;

        public AuthorizeAccountCommandHandler(
            IAccountRepository accountRepository,
            IHashGenerator hashGenerator,
            IJwtGenerator jwtGenerator)
        {
            _accountRepository = accountRepository;
            _hashGenerator = hashGenerator;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<Response<AuthorizeAccountResponse>> Handle(
            AuthorizeAccountRequest request,
            CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetWithEmailAsync(request.Email);

            if (account != null)
            {
                var passwordHash = await _hashGenerator.GenerateSaltedHash(request.Password);

                if (passwordHash.SequenceEqual(account.PasswordHash))
                {
                    var role = (UserRole)account.RoleId;
                    var token = _jwtGenerator.GenerateToken(
                        new[]
                        {
                                new Claim(WebApiClaimTypes.AccountId, account.Id.ToString()),
                                new Claim(WebApiClaimTypes.AccountRole, role.ToString())
                        });

                    return new Response<AuthorizeAccountResponse>
                    {
                        Result = new AuthorizeAccountResponse(token),
                        Status = ResponseStatus.Success
                    };
                }
            }

            return new Response<AuthorizeAccountResponse>
            {
                Status = ResponseStatus.Conflict
            };
        }
    }
}
