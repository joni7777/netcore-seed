using Bp.Config;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace BpSeed.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost
                .CreateDefaultBuilder<Startup>(args)
                .ConfigureAppConfiguration(ConfigureConfiguration.AddConfigurationByEnvironment);
    }
}