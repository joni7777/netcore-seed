using System;
using Microsoft.Extensions.Configuration;

namespace Bp.Logging.Sinks.Mattermost
{
    public class BpMattermostInfo
    {
        public string Protocol { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Path { get; set; }

        public bool IsValid() =>
            !String.IsNullOrEmpty(Protocol) && !String.IsNullOrEmpty(Host) &&
            !String.IsNullOrEmpty(Port) && !String.IsNullOrEmpty(Path);

        public const string MATTERMOST_MISSING_CONFIGURATION_MESSAGE = "Mattermost missing required configuration";

        public BpMattermostInfo()
        {
            
        }

        public BpMattermostInfo(string protocol, string host, string port, string path)
        {
            Protocol = protocol;
            Host = host;
            Port = port;
            Path = path;
        }
    }
}