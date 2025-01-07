using Microsoft.Extensions.DependencyInjection;

namespace ProjectManager.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAppMediatR(this IServiceCollection services)
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(MediatRRoot).Assembly);
            });
        }
    }
}
