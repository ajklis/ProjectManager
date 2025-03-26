namespace AuthenticationService.Contracts
{
    public interface IAuthenticationRequester
    {
        Task<Guid?> SendLoginRequestAsync(string email, string hashedPassword, CancellationToken cancellationToken = default);
        Task<Guid?> SendTokenRequest(Guid tokenId, CancellationToken cancellationToken = default);
        Task<int?> SendGetUserRequest(Guid tokenId, CancellationToken cancellationToken = default);
    }
}
