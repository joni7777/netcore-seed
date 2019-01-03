using System;
using System.Collections.Generic;
using System.Text;
using Serilog.Events;

namespace Bp.Logging.Sinks.Splunk
{
    public class BpSplunkLogFormatter
    {
        public static string Format(LogEvent logEvent)
        {
            var formattedLog = new StringBuilder();

            formattedLog.Append($"timestamp=\"{DateTime.Now.Millisecond.ToString()}\" ");
            formattedLog.Append($"severity=\"{logEvent.Level}\" ");
            formattedLog.Append($"action=\"{logEvent.RenderMessage()}\" ");

            foreach (KeyValuePair<string, LogEventPropertyValue> property in logEvent.Properties)
            {
                formattedLog.Append($"{property.Key}={property.Value} ");
            }
            
            return formattedLog.ToString();
        }
    }
}