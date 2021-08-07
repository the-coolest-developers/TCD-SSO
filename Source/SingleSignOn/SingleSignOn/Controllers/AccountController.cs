using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SingleSignOn.Commands;
using SingleSignOn.Requests;
using WebApiBaseLibrary.Authorization.Constants;
using WebApiBaseLibrary.Authorization.Extensions;
using WebApiBaseLibrary.Controllers;

namespace SingleSignOn.Controllers
{
    public class AccountController : BaseMediatorController
    {
        public AccountController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("register")]
        public async Task<ActionResult<Unit>> Register(
            [Required] [FromBody] RegisterAccount.RegisterAccountCommand registerAccountCommand)
        {
            return await ExecuteActionAsync(await Mediator.Send(registerAccountCommand));
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthorizeAccount.AuthorizeAccountResponse>> Login(
            [Required] [FromBody] AuthorizeAccount.AuthorizeAccountRequest authorizeAccountRequest)
            => await ExecuteActionAsync(await Mediator.Send(authorizeAccountRequest));

        [Authorize]
        [HttpGet("get")]
        public async Task<ActionResult<GetAccount.GetAccountResponse>> GetAccount()
        {
            var userId = Guid.Parse(User.GetClaim(WebApiClaimTypes.AccountId).Value);

            var getAccountRequest = new GetAccount.GetAccountRequest
            {
                AccountId = userId
            };

            return await ExecuteActionAsync(await Mediator.Send(getAccountRequest));
        }
    }
}