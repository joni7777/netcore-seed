using Bp.Config;
using Microsoft.AspNetCore.Hosting;

namespace BpSeed.API
{
    public class Runner
    {
        public static void Run()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>()
                .UseKestrel()
                .ConfigureAppConfiguration(ConfigureConfiguration.AddConfigurationByEnvironment);

            builder.Build().Run();
        }
    }
}