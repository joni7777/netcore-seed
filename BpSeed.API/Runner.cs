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
                .ConfigureAppConfiguration(ConfigureConfiguration.AddConfigurationByEnvironment)
                .UseKestrel((builderContext, options) =>
                {
                    options.Configure(builderContext.Configuration.GetSection("Kestrel"));
                });

            builder.Build().Run();
        }
    }
}