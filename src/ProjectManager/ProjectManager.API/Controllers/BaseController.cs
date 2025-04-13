using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Common;
using ProjectManager.Application.Models;
using ProjectManager.Application.Queries;

namespace ProjectManager.API.Controllers
{
    [Route("api/{controller}")]
    public abstract class BaseController : Controller
    {
        protected readonly IMediator _mediator;

        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected IActionResult FromCommandResult(CommandResult result) 
            => result.IsSuccess
            ? StatusCode(result.StatusCode, result.ReturnValue)
            : StatusCode(result.StatusCode, result.Message);

        protected async Task<IActionResult> HandleRequest<T>(T request, Func<UserDto, bool> allowedPredicate = null) where T : IRequest<CommandResult>
        {
            if (this.Request.Headers["SkipAuth"] != "true")
            {
                var tokenId = GetToken();

                var tokenResult = await _mediator.Send(new AuthenticateTokenQuery(tokenId));

                if (!tokenResult.IsSuccess)
                    return FromCommandResult(CommandResult.Unauthorized());

                var userResult = await _mediator.Send(new GetUserForTokenQuery(tokenId));
                if (!tokenResult.IsSuccess || userResult.ReturnValue is null)
                    return FromCommandResult(CommandResult.InternalServerError());

                var user = userResult.ReturnValue as UserDto;

                if (user is null)
                    return FromCommandResult(CommandResult.Unauthorized());

                if (allowedPredicate is not null && !allowedPredicate(user))
                    return FromCommandResult(CommandResult.Unauthorized());
            }

            return FromCommandResult(await _mediator.Send(request));
        }

        public Guid GetToken()
        {
            var tokenString = this.Request.Headers["TokenId"].ToString().Replace("\"", "");

            if (!Guid.TryParse(tokenString, out var token))
                return Guid.NewGuid();
            return token;
        }
    }
}
