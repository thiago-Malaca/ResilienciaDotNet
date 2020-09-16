using System;
using System.IO;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Serilog;

using Steeltoe.Extensions.Configuration.ConfigServer;

namespace Bacen
{
    public class Program
    {

        public static IConfiguration Configuration;

        public static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory() + "/config/")
                .AddJsonFile("appsettings.json", optional : false, reloadOnChange : true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional : false, reloadOnChange : true)
                .AddEnvironmentVariables()
                .AddConfigServer()
                .Build();

            /* Adicionado a estrutura do try/catch abaixo para captura os erros de inicialização,
               erros que normalmente geram dor de cabeça para resolver. */
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            try
            {
                Log.Information("Starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseContentRoot(Directory.GetCurrentDirectory() + "/config/");
                webBuilder.AddConfigServer();
                webBuilder.UseStartup<Startup>();
            });
    }
}