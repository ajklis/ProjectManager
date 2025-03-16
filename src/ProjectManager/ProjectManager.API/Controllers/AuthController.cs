using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.API.Models;
using ProjectManager.Application.Queries;
using ProjectManager.Domain.Models;

namespace ProjectManager.API.Controllers
{
    public class AuthController : BaseController
    {
        public AuthController(IMediator mediator) : base(mediator) { }

        [HttpGet("user")]
        public async Task<IActionResult> AuthorizeUser([FromBody] AuthUserModel model)
            => FromCommandResult(await _mediator.Send(new AuthenticateUserQuery(model.Email, model.Password)));

        [HttpGet("token")]
        public async Task<IActionResult> AuthorizeToken([FromBody] AuthTokenModel model)
            => FromCommandResult(await _mediator.Send(new AuthenticateTokenQuery(Guid.TryParse(model.TokenId, out Guid result) ? result : Guid.NewGuid())));
    }
}
