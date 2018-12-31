using Bp.Logging.Sinks.Mattermost;
using Bp.Logging.Sinks.Splunk;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog.Events;

namespace Bp.Logging
{
    public static class SerilogInit
    {
        public static void ConfigureLogger(WebHostBuilderContext builder, LoggerConfiguration configuration)
        {
            configuration
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.WithProperty("ServiceName", builder.Configuration.GetSection("Service")["name"]);

            var splunkLogger = builder.Configuration.GetSection("SplunkLogger");
            if (splunkLogger.Exists())
            {
                configuration.WriteTo.BpSplunkSink(new BpSplunkInfo(splunkLogger));
            }

            var mattermostLogger = builder.Configuration.GetSection("MattermostLogger");
            if (mattermostLogger.Exists())
            {
                configuration.WriteTo.BpMattermostSink(new BpMattermostInfo(mattermostLogger),
                    restrictedToMinimumLevel: LogEventLevel.Error);
            }
        }
    }
}