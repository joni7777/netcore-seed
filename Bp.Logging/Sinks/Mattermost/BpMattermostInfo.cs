using System;
using Microsoft.Extensions.Configuration;

namespace Bp.Logging.Sinks.Mattermost
{
    public class BpMattermostInfo
    {
        public string Host { get; }
        public string Path { get; }
        
        private const string MATTERMOST_MISSING_CONFIGURATION_MESSAGE = "Mattermost configuration require {0}";

        public BpMattermostInfo(IConfigurationSection loggerConfiguration)
            :this(
                loggerConfiguration["Host"],
                loggerConfiguration["Path"])
        {
        }
        
        public BpMattermostInfo(string host, string path)
        {
            Host = host ?? throw new ArgumentNullException(nameof(host), string.Format(MATTERMOST_MISSING_CONFIGURATION_MESSAGE, "host name"));
            Path = path ?? throw new ArgumentNullException(nameof(path), string.Format(MATTERMOST_MISSING_CONFIGURATION_MESSAGE, "url path"));
        }
    }
}