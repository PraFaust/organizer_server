using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using organizer_server.Models;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;


namespace organizer_server
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<UserContext>();
                    InitData.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

            // This is fuck
            .ConfigureAppConfiguration((context, config) =>
            {
                var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
                config.AddAzureKeyVault(
                keyVaultEndpoint,
                new DefaultAzureCredential());
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

        // This is work fine
        /*.ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });*/
    }
}
