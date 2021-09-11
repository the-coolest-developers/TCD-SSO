using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SingleSignOn.Commands.RegisterAccount;
using SingleSignOn.Requests.AuthorizeAccount;
using SingleSignOn.Requests.GetAccount;
using WebApiBaseLibrary.Authorization.Constants;
using WebApiBaseLibrary.Authorization.Extensions;
using WebApiBaseLibrary.Controllers;

namespace SingleSignOn.Controllers
{
    [ApiController]
    public class AccountController : BaseMediatorController
    {
        public AccountController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("register")]
        public async Task<ActionResult<Unit>> Register(
            [Required] [FromBody] RegisterAccountCommand registerAccountCommand)
        {
            return await ExecuteActionAsync(await Mediator.Send(registerAccountCommand));
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthorizeAccountResponse>> Login(
            [Required] [FromBody] AuthorizeAccountRequest authorizeAccountRequest)
            => await ExecuteActionAsync(await Mediator.Send(authorizeAccountRequest));

        [Authorize]
        [HttpGet("get")]
        public async Task<ActionResult<GetAccountResponse>> GetAccount()
        {
            var userId = Guid.Parse(User.GetClaim(WebApiClaimTypes.AccountId).Value);

            var getAccountRequest = new GetAccountRequest
            {
                AccountId = userId
            };

            return await ExecuteActionAsync(await Mediator.Send(getAccountRequest));
        }
    }
}