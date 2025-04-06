using AuthenticationService.Options;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using ProjectManager.Infrastructure.Extensions;

namespace AuthenticationService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        //Console.WriteLine("Auth channel: " + builder.Configuration["AuthenticationOptions:AuthenticationChannel"]);
        //.WebHost.UseUrls(builder.Configuration["AuthenticationOptions:AuthenticationChannel"]);
        builder.WebHost.UseUrls("http://0.0.0.0:6000"); 
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ConfigureEndpointDefaults(def =>
            {
                def.Protocols = HttpProtocols.Http1;
            });
        });

        builder.Services.AddDatabase(builder.Configuration);

        builder.Services.Configure<AuthenticationOptions>(builder.Configuration.GetSection(AuthenticationOptions.OptionsKey));
        builder.Services.AddSingleton<TokenStore>();
        builder.Services.AddGrpc();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<GreeterService>();
        app.MapGrpcService<AuthenticationManager>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}