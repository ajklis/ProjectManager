using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ProjectManager.Domain.Contracts;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Models;
using ProjectManager.Infrastructure.Options;

namespace ProjectManager.Infrastructure.Services
{
    public sealed class AuthenticationService : BackgroundService, IAuthenticationService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IOptions<AuthenticationOptions> _options;

        private List<AccessToken> tokenStore = new();

        public AuthenticationService(IServiceScopeFactory scopeFactory, IOptions<AuthenticationOptions> options)
        {
            _scopeFactory = scopeFactory;
            _options = options;
        }

        public async Task<AccessToken?> AuthenticateUserAsync(string email, string hashedPassword)
        {
            var repo = GetUserRepo();
            var user = await repo.GetUserByEmail(email);
            if (user is null || user.HashedPassword != hashedPassword)
                return null;

            AccessToken token;
            if (tokenStore.Where(x => x.UserId == user.Id).Any())
            {
                token = tokenStore.Where(x => x.UserId == user.Id).FirstOrDefault();

                token.ExpiresAt = DateTime.Now + _options.Value.AccessTokenLifeTime;
                return token;
            }
            else
                token = CreateToken(user);

            tokenStore.Add(token);
            return token;
        }

        public async Task<AccessToken?> AuthenticateTokenAsync(Guid tokenId)
        {
            var token = tokenStore.Where(x => x.TokenId == tokenId).FirstOrDefault();
            if (token is null || token.ExpiresAt < DateTime.Now)
            {
                tokenStore.Remove(token);
                return null;
            }

            token.ExpiresAt = DateTime.Now + _options.Value.AccessTokenLifeTime;

            return token;
        }

        public async Task<User?> GetUserForTokenAsync(Guid tokenId)
        {
            var repo = GetUserRepo();
            var token = tokenStore.Find(x => x.TokenId == tokenId);
            if (token is null)
                return null;

            var user = await repo.GetUserById(token.UserId);
            return user;
        }

        private AccessToken CreateToken(User user)
        {
            return new AccessToken
            {
                TokenId = Guid.NewGuid(),
                UserId = user.Id,
                ExpiresAt = DateTime.Now + _options.Value.AccessTokenLifeTime
            };
        }

        private IUserRepository GetUserRepo()
        {
            var scope = _scopeFactory.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IUserRepository>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                tokenStore.RemoveAll(x => x.ExpiresAt < DateTime.Now);
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
