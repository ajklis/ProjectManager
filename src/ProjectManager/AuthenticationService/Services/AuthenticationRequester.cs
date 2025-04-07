using AuthenticationService.Contracts;
using AuthenticationService.Models;
using AuthenticationService.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

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

            _logger.LogDebug("{o}", _options.Value.AuthenticationUrl);
        }

        public async Task<Guid?> SendLoginRequestAsync(string email, string hashedPassword, CancellationToken cancellationToken = default)
        {
            var requestBody = new LoginRequest
            {
                Email = email,
                HashedPassword = hashedPassword
            };

            _logger.LogDebug("{o}", _options.Value.AuthenticationUrl);

            using (var client = new HttpClient())
            {
                var request = CreateJsonRequest($"{_options.Value.AuthenticationUrl}/auth/user", requestBody);

                try
                {
                    var response = await client.SendAsync(request);
                    var content = await response.Content.ReadAsStringAsync();
                    return Guid.Parse(content);
                }
                catch (Exception e)
                {
                    _logger.LogDebug("{e}", e.Message);
                    return null;
                }
            }
        }

        public async Task<Guid?> SendTokenRequest(Guid tokenId, CancellationToken cancellationToken = default)
        {
            var requestBody = new TokenRequest
            {
                Token = tokenId.ToString()
            };

            using (var client = new HttpClient())
            {
                var request = CreateJsonRequest($"{_options.Value.AuthenticationUrl}/auth/token", requestBody);

                try
                {
                    var response = await client.SendAsync(request);
                    var content = await response.Content.ReadAsStringAsync();
                    return Guid.Parse(content);
                }
                catch (Exception e)
                {
                    _logger.LogDebug("{e}", e.Message);
                    return null;
                }
            }
        }

        public async Task<int?> SendGetUserRequest(Guid tokenId, CancellationToken cancellationToken = default)
        {
            var requestBody = new TokenRequest
            {
                Token = tokenId.ToString()
            };

            using (var client = new HttpClient())
            {
                var request = CreateJsonRequest($"{_options.Value.AuthenticationUrl}/auth/getUser", requestBody);

                try
                {
                    var response = await client.SendAsync(request);
                    var content = await response.Content.ReadAsStringAsync();
                    return int.Parse(content);
                }
                catch (Exception e)
                {
                    _logger.LogDebug("{e}", e.Message);
                    return null;
                }
            }
        }

        private HttpRequestMessage CreateJsonRequest(string url, object content)
        {
            var request = new HttpRequestMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json"),
                Method = HttpMethod.Post,
                RequestUri = new Uri(url)
            };

            return request;
        }
    }
}
