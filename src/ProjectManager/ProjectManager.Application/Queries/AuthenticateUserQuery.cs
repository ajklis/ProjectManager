using MediatR;
using ProjectManager.Application.Common;
using ProjectManager.Domain.Contracts;

namespace ProjectManager.Application.Queries
{
    public record AuthenticateUserQuery(string Email, string Password) : IRequest<CommandResult>;

    public class AuthenticateUserQueryHandler : IRequestHandler<AuthenticateUserQuery, CommandResult>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IPasswordHashingService _passwordHashingService;

        public AuthenticateUserQueryHandler(IAuthenticationService authenticationService, IPasswordHashingService passwordHashingService)
        {
            _authenticationService = authenticationService;
            _passwordHashingService = passwordHashingService;
        }

        public async Task<CommandResult> Handle(AuthenticateUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var hashedPassword = _passwordHashingService.GetHashedPassword(request.Password);
                
                var token = await _authenticationService.AuthenticateUserAsync(request.Email, hashedPassword);

                return token is not null
                    ? CommandResult.Success(token.TokenId)
                    : CommandResult.Failed("Incorrect email or password", 401);
            }
            catch (Exception e)
            {
                return CommandResult.InternalServerError(e);
            }
        }
    }
}
