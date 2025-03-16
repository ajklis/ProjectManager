using MediatR;
using ProjectManager.Application.Common;
using ProjectManager.Domain.Contracts;

namespace ProjectManager.Application.Queries
{
    public record GetUserForTokenQuery(Guid TokenId) : IRequest<CommandResult>;

    public class GetUserForTokenQueryHandler : IRequestHandler<GetUserForTokenQuery, CommandResult>
    {
        private readonly IAuthenticationService _authenticationService;

        public GetUserForTokenQueryHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<CommandResult> Handle(GetUserForTokenQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _authenticationService.GetUserForTokenAsync(request.TokenId);

                return user is not null
                    ? CommandResult.Success(user)
                    : CommandResult.Failed("User not found", 404);
            }
            catch (Exception e)
            {
                return CommandResult.InternalServerError(e);
            }
        }
    }
}
