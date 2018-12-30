using System;
using System.Collections.Generic;
using Serilog.Events;

namespace Bp.Logging.Sinks.Splunk
{
    public class BpSplunkLogFormatter : BpLogFormatter
    {
        public override string Format(LogEvent logEvent)
        {
            var formattedLog = "";

            formattedLog += $"timestamp={DateTime.Now.Millisecond.ToString()} ";
            formattedLog += $"severity={logEvent.Level} ";
            formattedLog += $"action={logEvent.RenderMessage()} ";

            foreach (KeyValuePair<string, LogEventPropertyValue> property in logEvent.Properties)
            {
                formattedLog += $"{property.Key}={property.Value} ";
            }

            return formattedLog;
        }
    }
}