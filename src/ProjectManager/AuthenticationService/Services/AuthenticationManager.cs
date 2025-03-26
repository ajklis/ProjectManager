using AuthenticationService.Options;
using Grpc.Core;
using Microsoft.Extensions.Options;
using ProjectManager.Domain.Contracts;
using ProjectManager.Domain.Models;

namespace AuthenticationService.Services
{
    internal sealed class AuthenticationManager : AuthService.AuthServiceBase
    {
        private readonly ILogger<AuthenticationManager> _logger;
        private readonly IUserRepository _userRepo;
        private readonly TokenStore _store;
        private readonly IOptions<AuthenticationOptions> _options;

        public AuthenticationManager(ILogger<AuthenticationManager> logger, IUserRepository userRepo, 
            TokenStore store, IOptions<AuthenticationOptions> options)
        {
            _logger = logger;
            _userRepo = userRepo;
            _store = store;
            _options = options;
        }

        public override async Task<TokenResponse> AuthenticateUser(LoginRequest request, ServerCallContext context)
        {
            var user = await _userRepo.GetUserByEmail(request.Email);
            if (user is null || request.HashedPassword != user.HashedPassword)
                return EmptyResponse;

            var token = _store.Get(user.Id);
            if (token is not null)
            {
                token.ExpiresAt = ExpirationDate;
                return FromToken(token);
            }

            token = CreateToken(user.Id);
            _store.Add(token);
            return FromToken(token);
        }

        public override async Task<TokenResponse> AuthenticateToken(TokenRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.Token, out var tokenId))
                return EmptyResponse;

            var token = _store.Get(tokenId);
            _logger.LogInformation("Requested token {id}", token.TokenId);
            if (token is null || token.ExpiresAt < DateTime.Now)
                return EmptyResponse;

            token.ExpiresAt = ExpirationDate;
            return FromToken(token);
        }

        public override async Task<UserResponse> GetUser(TokenRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.Token, out var tokenId))
                return EmptyUserResponse;

            var token = _store.Get(tokenId);

            if (token is null)
                return EmptyUserResponse;

            return new UserResponse { Id = token.UserId, Found = true };
        }

        private TokenResponse FromToken(AccessToken token) => new TokenResponse { Token = token.TokenId.ToString() };

        private AccessToken CreateToken(int userId)
            => new AccessToken
            {
                TokenId = Guid.NewGuid(),
                UserId = userId,
                ExpiresAt = DateTime.Now.Add(_options.Value.AccessTokenLifeTime)
            };
        private TokenResponse EmptyResponse => new TokenResponse { Token = "" };

        private UserResponse EmptyUserResponse => new UserResponse
        {
            Id = -1,
            Found = false
        };

        private DateTime ExpirationDate => DateTime.Now.Add(_options.Value.AccessTokenLifeTime);
    }
}
