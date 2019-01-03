using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BpSeed.Isolate
{

    public abstract class IsolateAction
    {
        protected IsolateAction(IHostingEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        protected IHostingEnvironment Environment { get; }
        protected IConfiguration Configuration { get; }

        public async Task RunAsync()
        {
            var isolatedConfiguration = await GenerateIsolateConfigAsync();

            await SaveGeneratedIsolateConfigAsync(isolatedConfiguration);
        }

        protected abstract Task<JObject> GenerateIsolateConfigAsync();

        private async Task SaveGeneratedIsolateConfigAsync(JObject isolatedConfiguration)
        {
            var json = isolatedConfiguration.ToString();
            Console.WriteLine($"ContentRootPath: {Environment.ContentRootPath}");
            using (var writer = File.CreateText(Path.Combine(Environment.ContentRootPath, "config", $"{Environment.EnvironmentName}.json")))
            {
                await writer.WriteAsync(json);
            }
        }
    }

    public class DefaultIsolate : IsolateAction
    {
        public DefaultIsolate(IHostingEnvironment environment, IConfiguration configuration) : base(environment, configuration)
        {
        }

        protected override Task<JObject> GenerateIsolateConfigAsync()
        {
            return Task.FromResult(JObject.Parse("{}"));
        }
    }

    public class MongoIsolate : IsolateAction
    {
        public MongoIsolate(IHostingEnvironment environment, IConfiguration configuration) : base(environment, configuration)
        {
        }

        protected override Task<JObject> GenerateIsolateConfigAsync()
        {
            return null;
        }
    }
}
