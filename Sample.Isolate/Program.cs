using BBp.Isolate
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Sample.Isolate
{
    class Program
    {
        static Task Main(string[] args)
        {
            args = new[] { "autoenv" };
            var builder = IsolateBuilder.For<ServiceIsolate>(args)
                .ConfigureServices(services => { /* some service */ });

            return builder.Build().RunAsync();
        }
    }

    public class ServiceIsolate : IsolateAction
    {
        public ServiceIsolate(IHostingEnvironment environment, IConfiguration configuration) : base(environment, configuration)
        {
        }

        protected override Task<JObject> GenerateIsolateConfigAsync()
        {
            var port = Configuration.GetValue<int>("port");
            return Task.FromResult(JObject.FromObject(new
            {
                partition = "Blachman",
                newPort = port
            }));
        }
    }
}
