using Bp.Config;
using Bp.Logging;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace Bp.ApiRunner
{
    public class Runner
    {
        public static void Run()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>()
                .UseKestrel()
                .ConfigureAppConfiguration(ConfigureConfiguration.AddConfigurationByEnvironment)
                .UseSerilog(SerilogInit.ConfigureLogger);;

            builder.Build().Run();
        }
    }
}