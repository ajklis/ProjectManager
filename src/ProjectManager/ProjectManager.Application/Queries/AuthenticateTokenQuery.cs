using MediatR;
using ProjectManager.Application.Common;
using ProjectManager.Domain.Contracts;

namespace ProjectManager.Application.Queries
{
    public record AuthenticateTokenQuery(Guid TokenId) : IRequest<CommandResult>;

    public class AuthenticateTokenQueryHandler : IRequestHandler<AuthenticateTokenQuery, CommandResult>
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticateTokenQueryHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<CommandResult> Handle(AuthenticateTokenQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _authenticationService.AuthenticateTokenAsync(request.TokenId);

                return token is not null
                    ? CommandResult.Success(token.TokenId)
                    : CommandResult.Failed("Not authorized", 401);
            }
            catch (Exception e)
            {
                return CommandResult.InternalServerError(e);
            }
        }
    }
}
