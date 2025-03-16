using Microsoft.Extensions.Hosting;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Models;

namespace ProjectManager.Domain.Contracts
{
    public interface IAuthenticationService : IHostedService
    {
        public Task<AccessToken?> AuthenticateUserAsync(string email, string hashedPassword);
        public Task<AccessToken?> AuthenticateTokenAsync(Guid tokenId);
        public Task<User?> GetUserForTokenAsync(Guid tokenId);
    }
}
