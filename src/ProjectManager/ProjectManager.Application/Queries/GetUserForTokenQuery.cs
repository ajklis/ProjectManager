using AuthenticationService.Contracts;
using MediatR;
using Newtonsoft.Json;
using ProjectManager.Application.Common;
using ProjectManager.Application.Models;

namespace ProjectManager.Application.Queries
{
    public record GetUserForTokenQuery(Guid TokenId) : IRequest<CommandResult>;

    public class GetUserForTokenQueryHandler : IRequestHandler<GetUserForTokenQuery, CommandResult>
    {
        private readonly IAuthenticationRequester _auth;
        private readonly IMediator _mediator;

        public GetUserForTokenQueryHandler(IAuthenticationRequester auth, IMediator mediator)
        {
            _auth = auth;
            _mediator = mediator;
        }

        public async Task<CommandResult> Handle(GetUserForTokenQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine(request.TokenId);
                var userId = await _auth.SendGetUserRequest(request.TokenId);

                Console.WriteLine($"User id for token {request.TokenId}: {userId}");

                if (userId is null)
                    return CommandResult.Failed("User not found", 404);

                var user = await _mediator.Send(new GetUserByIdQuery((int)userId), cancellationToken);

                Console.WriteLine(JsonConvert.SerializeObject(user));

                return user is not null
                    ? CommandResult.Success(user.ReturnValue as UserDto)
                    : CommandResult.Failed("User not found", 404);
            }
            catch (Exception e)
            {
                return CommandResult.InternalServerError(e);
            }
        }
    }
}
