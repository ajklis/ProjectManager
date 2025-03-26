using AuthenticationService.Contracts;
using AuthenticationService.Options;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;

namespace AuthenticationService.Services
{
    public sealed class AuthenticationRequester : IAuthenticationRequester
    {
        private readonly IOptions<AuthenticationOptions> _options;
        private readonly ILogger<AuthenticationRequester> _logger;

        public AuthenticationRequester(IOptions<AuthenticationOptions> options, ILogger<AuthenticationRequester> logger)
        {
            _options = options;
            _logger = logger;
        }

        public async Task<Guid?> SendLoginRequestAsync(string email, string hashedPassword, CancellationToken cancellationToken = default)
        {
            var request = new LoginRequest
            {
                Email = email,
                HashedPassword = hashedPassword
            };
            var handler = new SocketsHttpHandler
            {
                EnableMultipleHttp2Connections = true
            };

            using (var channel = GetChannel())
            {
                var client = new AuthService.AuthServiceClient(channel);
                _logger.LogDebug("{c}", _options.Value.AuthenticationChannel);
                _logger.LogDebug("{e} {p}", email, hashedPassword);
                try
                {
                    var response = await client.AuthenticateUserAsync(request, cancellationToken: cancellationToken);
                    if (string.IsNullOrEmpty(response.Token))
                        return null;
                    return Guid.Parse(response.Token);
                }
                catch (Exception e)
                {
                    _logger.LogDebug("Error while sending login request: {e}", e.ToString());
                    return null;
                }
            }
        }

        public async Task<Guid?> SendTokenRequest(Guid tokenId, CancellationToken cancellationToken = default)
        {
            var request = new TokenRequest
            {
                Token = tokenId.ToString()
            };

            using (var channel = GrpcChannel.ForAddress(_options.Value.AuthenticationChannel))
            {
                var client = new AuthService.AuthServiceClient(channel);
                try
                {
                    var response = await client.AuthenticateTokenAsync(request, cancellationToken: cancellationToken);
                    if (string.IsNullOrEmpty(response.Token))
                        return null;
                    return Guid.Parse(response.Token);
                }
                catch (Exception e)
                {
                    _logger.LogDebug("Error while sending token request: {e}", e.ToString());
                    return null;
                }
            }
        }

        public async Task<int?> SendGetUserRequest(Guid tokenId, CancellationToken cancellationToken = default)
        {
            var request = new TokenRequest
            {
                Token = tokenId.ToString()
            };

            using (var channel = GrpcChannel.ForAddress(_options.Value.AuthenticationChannel))
            {
                var client = new AuthService.AuthServiceClient(channel);
                try
                {
                    var response = await client.GetUserAsync(request, cancellationToken: cancellationToken);
                    if (!response.Found)
                        return null;
                    return response.Id;
                }
                catch (Exception e)
                {
                    _logger.LogDebug("Error while getting user for token: {e}", e.ToString());
                    return null;
                }
            }
        }

        private GrpcChannel GetChannel()
        {
            var handler = new SocketsHttpHandler
            {
                EnableMultipleHttp2Connections = true
            };
            var channel = GrpcChannel.ForAddress(_options.Value.AuthenticationChannel,
                new GrpcChannelOptions
                {
                    HttpClient = new HttpClient(handler),
                    Credentials = ChannelCredentials.Insecure
                });

            return channel;
        }
    }
}
