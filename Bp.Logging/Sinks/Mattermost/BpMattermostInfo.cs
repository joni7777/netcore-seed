using System;
using Microsoft.Extensions.Configuration;

namespace Bp.Logging.Sinks.Mattermost
{
    public class BpMattermostInfo
    {
        public string Host { get; set; }
        public string Path { get; set; }

        public bool IsValid() => !String.IsNullOrEmpty(Host) && !String.IsNullOrEmpty(Path);

        public const string MATTERMOST_MISSING_CONFIGURATION_MESSAGE = "Mattermost missing required configuration";

        public BpMattermostInfo(){}

        public BpMattermostInfo(string host, string path)
        {
            Host = host;
            Path = path;
        }
    }
}