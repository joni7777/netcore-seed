using System;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bp.HealthChecks
{
    public class ConfigureHealthChecks
    {
        public static void ConfigureBpHealthChecksServices(IServiceCollection services, IConfiguration configuration)
        {
            IHealthChecksBuilder healthChecks = services.AddHealthChecks();

            // TODO: Use real config path
            string sqlConfig = configuration["Data:ConnectionStrings:Sql"];
            if (sqlConfig != null)
            {
                healthChecks.AddSqlServer(sqlConfig, name: "sql-health", tags: new[] {HealthCheckTag.Data.ToString()});
            }

            string elasticConfig = configuration["Data:ConnectionStrings:Elastic"];
            if (elasticConfig != null)
            {
                healthChecks.AddElasticsearch(elasticConfig, name: "elastic-health", tags: new[] {HealthCheckTag.Data.ToString()});
            }

            string mongoConfig = configuration["Data:ConnectionStrings:Mongo"];
            if (mongoConfig != null)
            {
                healthChecks.AddMongoDb(mongoConfig, name: "mongo-health", tags: new[] {HealthCheckTag.Data.ToString()});
            }

            healthChecks.AddUrlGroup(
                new Uri("http://localhost:5000/swagger/v1/swagger.json"),
                name: "Get swagger.json",
                tags: new[] {HealthCheckTag.Sanity.ToString()});

            services.AddMvc().AddApplicationPart(Assembly.GetExecutingAssembly());
        }

        public static void UseBpHealthChecks(IApplicationBuilder app)
        {
            app.UseHealthChecks("/api/system/sanity", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains(HealthCheckTag.Sanity.ToString())
            });

            app.UseHealthChecks("/api/system/data", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains(HealthCheckTag.Data.ToString())
            });
        }
    }
}