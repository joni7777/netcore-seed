﻿using Bp.Logging.Sinks.Mattermost;
using Bp.Logging.Sinks.Splunk;
using Serilog;
using Microsoft.AspNetCore.Hosting;
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
            if (splunkLogger != null)
            {
                configuration.WriteTo.BpSplunkSink(
                    new BpSplunkInfo(
                        splunkLogger["Host"],
                        splunkLogger["Port"],
                        splunkLogger["Username"],
                        splunkLogger["Password"],
                        splunkLogger["SourceType"],
                        splunkLogger["Index"]));
            }

            var mattermostLogger = builder.Configuration.GetSection("MattermostLogger");
            if (mattermostLogger != null)
            {
                configuration.WriteTo.BpMattermostSink(
                    new BpMattermostInfo(
                        mattermostLogger["Host"],
                        mattermostLogger["Path"]),
                    restrictedToMinimumLevel: LogEventLevel.Error);
            }
        }
    }
}