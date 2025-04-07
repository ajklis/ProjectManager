using AuthenticationService.Contracts;
using AuthenticationService.Options;
using AuthenticationService.Services;
using ProjectManager.Infrastructure.Extensions;

namespace AuthenticationService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var url = builder.Configuration["AuthenticationOptions:AuthenticationSetupUrl"];
        Console.WriteLine(url);
        builder.WebHost.UseUrls(url); 

        builder.Services.AddDatabase(builder.Configuration);

        builder.Services.Configure<AuthenticationOptions>(builder.Configuration.GetSection(AuthenticationOptions.OptionsKey));
        builder.Services.AddSingleton<TokenStore>();
        builder.Services.AddSingleton<IAuthenticator, AuthenticatorService>();

        /*builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", policy =>
                policy.WithOrigins("*")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
            );
        });*/

        builder.Services.AddControllers();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseHttpsRedirection();
        app.MapControllers();
        //app.UseCors("AllowSpecificOrigin");
        app.MapGet("/", () => "OK");

        app.Run();
    }
}