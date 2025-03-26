using AuthenticationService.Contracts;
using MediatR;
using ProjectManager.Application.Common;

namespace ProjectManager.Application.Queries
{
    public record AuthenticateTokenQuery(Guid TokenId) : IRequest<CommandResult>;

    public class AuthenticateTokenQueryHandler : IRequestHandler<AuthenticateTokenQuery, CommandResult>
    {
        private readonly IAuthenticationRequester _auth;

        public AuthenticateTokenQueryHandler(IAuthenticationRequester auth)
        {
            _auth = auth;
        }

        public async Task<CommandResult> Handle(AuthenticateTokenQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _auth.SendTokenRequest(request.TokenId);

                return token is not null
                    ? CommandResult.Success(token)
                    : CommandResult.Failed("Not authorized", 401);
            }
            catch (Exception e)
            {
                return CommandResult.InternalServerError(e);
            }
        }
    }
}
