using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.API.Models;
using ProjectManager.Application.Queries;
using System.Net;

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
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var handler = new HttpClientHandler
            {
                SslProtocols = System.Security.Authentication.SslProtocols.None
            };
            using var client = new HttpClient(handler);
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:6000/")
            {
                Version = HttpVersion.Version11
            };
            var response = await client.SendAsync(request);
            return Ok(response);
        }
    }
}
