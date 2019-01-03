using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bp.HealthChecks
{
    public class ConfigureHealthChecks
    {
        public static void ConfigureBpHealthChecksService(IServiceCollection services, IConfiguration configuration)
        {
            IHealthChecksBuilder healthChecks = services.AddHealthChecks();

            string sqlConfig = configuration["Data:ConnectionStrings:Sql"];
            if (sqlConfig != null)
            {
                healthChecks.AddSqlServer(sqlConfig, tags: new[] {HealthCheckTag.Data.ToString()});
            }

            string elasticConfig = configuration["Data:ConnectionStrings:Elastic"];
            if (elasticConfig != null)
            {
                healthChecks.AddElasticsearch(elasticConfig, tags: new[] {HealthCheckTag.Data.ToString()});
            }

            string mongoConfig = configuration["Data:ConnectionStrings:Mongo"];
            if (mongoConfig != null)
            {
                healthChecks.AddMongoDb(mongoConfig, tags: new[] {HealthCheckTag.Data.ToString()});
            }

            healthChecks.AddUrlGroup(
                new Uri("http://localhost:5000/swagger/v1/swagger.json"),
                tags: new[] {HealthCheckTag.Sanity.ToString()});
        }

        public static void UseBpHealthChecks(IApplicationBuilder app)
        {
            app.UseHealthChecks("/system/sanity", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains(HealthCheckTag.Sanity.ToString())
            });

            app.UseHealthChecks("/system/data", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains(HealthCheckTag.Data.ToString())
            });
        }
    }
}