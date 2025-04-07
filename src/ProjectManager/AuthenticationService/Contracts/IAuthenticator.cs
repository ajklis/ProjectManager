using AuthenticationService.Models;
using ProjectManager.Domain.Models;

namespace AuthenticationService.Contracts
{
    public interface IAuthenticator
    {
        Task<AccessToken?> AuthenticateUser(LoginRequest request);
        Task<AccessToken?> AuthenticateToken(TokenRequest request);
        Task<int?> GetUser(TokenRequest request);
    }
}
