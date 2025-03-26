using AuthenticationService.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectManager.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAppMediatR(this IServiceCollection services, IConfiguration config)
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(MediatRRoot).Assembly);
            });
            services.AddAuthenticationRequester(config);
        }
    }
}
