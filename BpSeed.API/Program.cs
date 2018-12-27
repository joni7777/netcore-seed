using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace BpSeed.API
{
    public class Program
    {
        private const string CONFIG_FILE_BASE_NAME = "appsettings";

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(ConfigConfiguration)
                .UseStartup<Startup>();
        
        private static void ConfigConfiguration(WebHostBuilderContext hostingContext, IConfigurationBuilder config)
        {
            var environmentName = hostingContext.HostingEnvironment.EnvironmentName;
            config
                .SetBasePath($"{Directory.GetCurrentDirectory()}/Config")
                .AddJsonFile($"{CONFIG_FILE_BASE_NAME}.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"{CONFIG_FILE_BASE_NAME}.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"{CONFIG_FILE_BASE_NAME}.{environmentName}.Local.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }
    }
}