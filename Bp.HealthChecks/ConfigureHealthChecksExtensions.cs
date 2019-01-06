using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bp.HealthChecks
{
    public static class ConfigureHealthChecksExtensions
    {
        public static void ConfigureBpHealthChecksServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            IHealthChecksBuilder healthChecks = services.AddHealthChecks();

            // TODO: Use real config path
            string sqlConfig = configuration["Data:ConnectionStrings:Sql"];
            if (sqlConfig != null)
            {
                healthChecks.AddSqlServer(sqlConfig, name: "sql-health", tags: new[] {HealthCheckTag.DATA});
            }

            string elasticConfig = configuration["Data:ConnectionStrings:Elastic"];
            if (elasticConfig != null)
            {
                healthChecks.AddElasticsearch(elasticConfig, name: "elastic-health", tags: new[] {HealthCheckTag.DATA});
            }

            string mongoConfig = configuration["Data:ConnectionStrings:Mongo"];
            if (mongoConfig != null)
            {
                healthChecks.AddMongoDb(mongoConfig, name: "mongo-health", tags: new[] {HealthCheckTag.DATA});
            }

            // When there is redirect to https, the url group fails
            if (configuration["Kestrel:EndPoints:Https:Url"] != null)
            {
                healthChecks.AddUrlGroup(
                    new Uri($"{configuration["Kestrel:EndPoints:Http:Url"].Replace("*", "localhost")}/swagger/v1/swagger.json"),
                    name: "Get swagger.json",
                    tags: new[] {HealthCheckTag.SANITY});
            }
        }

        public static void UseBpHealthChecks(this IApplicationBuilder app)
        {
            app
                .ApplicationServices.GetService<ApplicationPartManager>()
                .ApplicationParts.Add(new AssemblyPart(Assembly.GetExecutingAssembly()));
        }
    }
}