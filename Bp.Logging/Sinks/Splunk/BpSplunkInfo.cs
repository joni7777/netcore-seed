using System;
using Microsoft.Extensions.Configuration;

namespace Bp.Logging.Sinks.Splunk
{
    public class BpSplunkInfo
    {
        public string Host { get; }
        public string Port { get; }
        public string Username { get; }
        public string Password { get; }
        public string Index { get; }
        public string SourceType { get; }

        private const string SPLUNK_MISSING_CONFIGURATION_MESSAGE = "Splunk configuration require {0}";

        public BpSplunkInfo(IConfigurationSection loggerConfiguration)
        :this(
            loggerConfiguration["Host"],
            loggerConfiguration["Port"],
            loggerConfiguration["Username"],
            loggerConfiguration["Password"],
            loggerConfiguration["SourceType"],
            loggerConfiguration["Index"])
        {
        }

        public BpSplunkInfo(string host, string port, string username, string password, string index, string sourceType)
        {
            Host = host ?? throw new ArgumentNullException(nameof(host), string.Format(SPLUNK_MISSING_CONFIGURATION_MESSAGE, "host name"));
            Port = port ?? throw new ArgumentNullException(nameof(port), string.Format(SPLUNK_MISSING_CONFIGURATION_MESSAGE, "port number"));
            Username = username ?? throw new ArgumentNullException(nameof(username), string.Format(SPLUNK_MISSING_CONFIGURATION_MESSAGE, "username"));
            Password = password ?? throw new ArgumentNullException(nameof(password), string.Format(SPLUNK_MISSING_CONFIGURATION_MESSAGE, "password"));
            Index = index ?? throw new ArgumentNullException(nameof(index), string.Format(SPLUNK_MISSING_CONFIGURATION_MESSAGE, "splunk index"));
            SourceType = sourceType ?? throw new ArgumentNullException(nameof(sourceType), string.Format(SPLUNK_MISSING_CONFIGURATION_MESSAGE, "splunk source type"));
        }
    }
}