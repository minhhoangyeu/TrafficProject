using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Traffic.Api.Logger;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Traffic.Data;

namespace Traffic
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            InitLogger();

            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    using (var context = new TrafficContext(services.GetRequiredService<DbContextOptions<TrafficContext>>()))
                    {
                        //await context.Database.MigrateAsync();
                        var dbInitializer = services.GetService<DbInitializer>();
                        dbInitializer.Seed().Wait();
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetService<ILogger<Program>>();
                    logger.LogError(ex, ex.Message);
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
                      .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                     .ReadFrom.Configuration(hostingContext.Configuration))
                     .ConfigureWebHostDefaults(webBuilder =>

                     {
                         webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                         webBuilder.UseStartup<Startup>();
                         webBuilder.UseIISIntegration();
                     });
        public static void InitLogger()
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Filter.ByExcluding(Matching.FromSource("Hangfire"))
            .Filter.ByExcluding(Matching.FromSource("Microsoft.EntityFrameworkCore.Model.Validation"))
            .Filter.ByExcluding(Matching.FromSource("Traffic.Api.Authorization.TrafficAuthenticationHandler"))
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.HttpsPolicy.HttpsRedirectionMiddleware"))
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager"))
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.DataProtection.Repositories.FileSystemXmlRepository"))
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
            .WriteTo.Console(new JsonFormatter())
            .Enrich.With(new RemovePropertiesEnricher())
            .CreateLogger();
        }
    }
}
