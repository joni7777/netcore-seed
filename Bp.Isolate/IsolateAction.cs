using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Bp.Isolate
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
            using (var writer = File.CreateText(Path.Combine(Environment.ContentRootPath, "config", $"{Environment.EnvironmentName}.json")))
            {
                await writer.WriteAsync(json);
            }
        }
    }
}
