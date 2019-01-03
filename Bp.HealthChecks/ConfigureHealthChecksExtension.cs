using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bp.HealthChecks
{
    public static class ConfigureHealthChecksExtension
    {
        public static void ConfigureBpHealthChecksService(this IServiceCollection services,
            IConfiguration configuration) =>
            ConfigureHealthChecks.ConfigureBpHealthChecksService(services, configuration);

        public static void UseBpHealthChecks(this IApplicationBuilder app) =>
            ConfigureHealthChecks.UseBpHealthChecks(app);
    }
}