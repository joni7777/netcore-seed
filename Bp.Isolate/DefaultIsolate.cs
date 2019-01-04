using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Bp.Isolate
{
    public class DefaultIsolate : IsolateAction
    {
        public DefaultIsolate(IHostingEnvironment environment, IConfiguration configuration) : base(environment, configuration)
        {
        }

        protected override Task<JObject> GenerateIsolateConfigAsync()
        {
            var config = new
            {
                
            };

            return Task.FromResult(JObject.FromObject(config));
        }
    }
}
