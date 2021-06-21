using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PartyMaker.EmailServiceHost;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PartyMaker.EmailService.Host
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));
            var environment = System.Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

            var pathToContentRoot = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Environment.SetEnvironmentVariable("LOGBASEDIR", $"{pathToContentRoot}");

            var builder = new HostBuilder()
                .UseEnvironment(environment)
                .ConfigureHostConfiguration((config) => { config.AddEnvironmentVariables(); })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(pathToContentRoot);
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    config.AddJsonFile($"configs{Path.DirectorySeparatorChar}queues.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: false);
                    config.AddJsonFile($"configs{Path.DirectorySeparatorChar}serilog.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: false);
                    config.AddJsonFile($"configs{Path.DirectorySeparatorChar}email.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: false);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    AppIocConfigure.ConfigureServices(services);
                    services.AddHostedService<EventListenerHost>();
                });

            await builder.RunConsoleAsync();
        }
    }
}
