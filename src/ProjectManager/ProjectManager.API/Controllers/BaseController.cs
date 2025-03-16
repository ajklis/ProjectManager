using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectManager.Application.Common;
using ProjectManager.Application.Queries;
using ProjectManager.Domain.Entities;

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

        protected async Task<IActionResult> HandleRequest<T>(Guid TokenId, T request, Func<User, bool> allowedPredicate = null) where T : IRequest<CommandResult>
        {
            var headers = this.Request.Headers;

            Console.WriteLine(JsonConvert.SerializeObject(headers));

            if (!Guid.TryParse(headers["TokenId"], out var tokenId))
                return FromCommandResult(CommandResult.Unauthorized());
            
            var tokenResult = await _mediator.Send(new AuthenticateTokenQuery(tokenId));

            if (!tokenResult.IsSuccess)
                return FromCommandResult(CommandResult.Unauthorized());

            var userResult = await _mediator.Send(new GetUserForTokenQuery(TokenId));
            if (!tokenResult.IsSuccess || userResult.ReturnValue is null)
                return FromCommandResult(CommandResult.InternalServerError());

            var user = userResult.ReturnValue as User;

            if (allowedPredicate is not null && !allowedPredicate(user))
                return FromCommandResult(CommandResult.Unauthorized());

            return FromCommandResult(await _mediator.Send(request));
        }
    }
}
