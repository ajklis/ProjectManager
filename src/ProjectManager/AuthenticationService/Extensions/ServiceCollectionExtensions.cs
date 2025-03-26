using AuthenticationService.Contracts;
using AuthenticationService.Options;
using AuthenticationService.Services;

namespace AuthenticationService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthenticationRequester(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IAuthenticationRequester, AuthenticationRequester>();
            services.Configure<AuthenticationOptions>(config.GetSection(AuthenticationOptions.OptionsKey));
            return services;
        }
    }
}
