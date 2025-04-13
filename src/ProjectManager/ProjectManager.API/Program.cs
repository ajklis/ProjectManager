using AuthenticationService.Extensions;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Extensions;
using ProjectManager.Infrastructure.Extensions;
using ProjectManager.Infrastructure.Persistence;
using System.Text.Json.Serialization;

namespace ProjectManager.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // configure enums for post jsons
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // add db context for app
            builder.Services.AddDatabase(builder.Configuration);

            // add mediatr for controllers
            builder.Services.AddAppMediatR(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                    policy.WithOrigins("*")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                );
            });
            builder.Services.AddAuthenticationRequester(builder.Configuration);

            var app = builder.Build();

            // migrate database
            SeedData.Initialize(app);

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.UseCors("AllowSpecificOrigin");

            app.Urls.Add("http://*:5000");

            app.Run();
        }
    }

    public static class SeedData
    {
        public static void Initialize(WebApplication app)
        {
            using (var serviceScope = app.Services.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                // auto migration
                context.Database.Migrate();
            }
        }
    }
}
