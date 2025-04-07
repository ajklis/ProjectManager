using AuthenticationService.Contracts;
using AuthenticationService.Models;
using AuthenticationService.Options;
using Microsoft.Extensions.Options;
using ProjectManager.Domain.Contracts;
using ProjectManager.Domain.Models;

namespace AuthenticationService.Services
{
    internal class AuthenticatorService : IAuthenticator
    {
        private readonly TokenStore _store;
        private readonly ILogger<AuthenticatorService> _logger;
        private readonly IOptions<AuthenticationOptions> _options;
        private readonly IUserRepository _userRepo;

        public AuthenticatorService(TokenStore store, ILogger<AuthenticatorService> logger, IOptions<AuthenticationOptions> options, IUserRepository userRepo)
        {
            _store = store;
            _logger = logger;
            _options = options;
            _userRepo = userRepo;
        }

        public async Task<AccessToken?> AuthenticateToken(TokenRequest request)
        {
            if (!Guid.TryParse(request.Token, out var tokenId))
                return null;

            var token = _store.Get(tokenId);
            _logger.LogInformation("Requested token {id}", token.TokenId);
            if (token is null || token.ExpiresAt < DateTime.Now)
                return null;

            token.ExpiresAt = ExpirationDate;
            return token;
        }

        public async Task<AccessToken?> AuthenticateUser(LoginRequest request)
        {
            var user = await _userRepo.GetUserByEmail(request.Email);
            if (user is null || request.HashedPassword != user.HashedPassword)
                return null;

            var token = _store.Get(user.Id);
            if (token is not null)
            {
                token.ExpiresAt = ExpirationDate;
                return token;
            }

            token = CreateToken(user.Id);
            _store.Add(token);
            return token;
        }

        public async Task<int?> GetUser(TokenRequest request)
        {
            if (!Guid.TryParse(request.Token, out var tokenId))
                return null;

            var token = _store.Get(tokenId);

            if (token is null)
                return null;

            return token.UserId;
        }

        private AccessToken CreateToken(int userId)
            => new AccessToken
            {
                TokenId = Guid.NewGuid(),
                UserId = userId,
                ExpiresAt = DateTime.Now.Add(_options.Value.AccessTokenLifeTime)
            };

        private DateTime ExpirationDate => DateTime.Now.Add(_options.Value.AccessTokenLifeTime);
    }
}
