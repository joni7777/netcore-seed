using System;
using Serilog.Events;

namespace Bp.Logging.Sinks.Mattermost
{
    public class BpMattermostLogFormatter:BpLogFormatter
    {
        private const string CODE_PREFIX = "```";
        
        public override string Format(LogEvent log)
        {
            string logMessage = "";

            logMessage += $"{Monospace(log.RenderMessage())} - ";

            return logMessage;
        }

        private string Bold(string message)
        {
            return $"**{message}**";
        }

        private string Hyperlink(string message, string link)
        {
            return $"[{message}]({link})";
        }

        private string Emoji(string name)
        {
            return $":{name}:";
        }

        private string Monospace(string message)
        {
            return $"`{message}`";
        }
        
        private string Code(string message, string lang)
        {
            return $"{CODE_PREFIX}{lang}{Environment.NewLine}{message}{Environment.NewLine}{CODE_PREFIX}";
        }
    }
}