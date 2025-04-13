using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.API.Models;
using ProjectManager.Application.Queries;

namespace ProjectManager.API.Controllers
{
    public class AuthController : BaseController
    {
        public AuthController(IMediator mediator) : base(mediator) { }

        [HttpPost("user")]
        public async Task<IActionResult> AuthorizeUser([FromBody] AuthUserModel model)
            => FromCommandResult(await _mediator.Send(new AuthenticateUserQuery(model.Email, model.Password)));

        [HttpPost("token")]
        public async Task<IActionResult> AuthorizeToken([FromBody] AuthTokenModel model)
            => FromCommandResult(await _mediator.Send(new AuthenticateTokenQuery(Guid.TryParse(model.TokenId, out Guid result) ? result : Guid.NewGuid())));

        [HttpGet("check")]
        public async Task<IActionResult> CheckAuthService()
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://auth:6000/");
            var response = await client.SendAsync(request);
            return Ok(response);
        }

        [HttpGet("userDetails")]
        public async Task<IActionResult> GetUserDetailsFromToken()
        {
            var headers = this.Request.Headers;
            var tokenString = headers["TokenId"].ToString().Replace("\"", "");

            return FromCommandResult(await _mediator.Send(new GetUserForTokenQuery(Guid.Parse(tokenString))));
        }
    }
}
