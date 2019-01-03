using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bp.HealthChecks
{
    public static class ConfigureHealthChecksExtension
    {
        public static void ConfigureBpHealthChecksServices(this IServiceCollection services,
            IConfiguration configuration) =>
            ConfigureHealthChecks.ConfigureBpHealthChecksServices(services, configuration);

        public static void UseBpHealthChecks(this IApplicationBuilder app) =>
            ConfigureHealthChecks.UseBpHealthChecks(app);
    }
}