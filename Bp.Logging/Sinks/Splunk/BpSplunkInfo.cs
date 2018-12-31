using System;

namespace Bp.Logging.Sinks.Splunk
{
    public class BpSplunkInfo
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Index { get; set; }
        public string SourceType { get; set; }

        public bool IsValid() =>
            !String.IsNullOrEmpty(Host) && !String.IsNullOrEmpty(Port) &&
            !String.IsNullOrEmpty(Username) && !String.IsNullOrEmpty(Password) &&
            !String.IsNullOrEmpty(Index) && !String.IsNullOrEmpty(SourceType);

        public const string SPLUNK_MISSING_CONFIGURATION_MESSAGE = "Splunk missing required configuration";

        public BpSplunkInfo(){}

        public BpSplunkInfo(string host, string port, string username, string password, string index, string sourceType)
        {
            Host = host;
            Port = port;
            Username = username;
            Password = password;
            Index = index;
            SourceType = sourceType;
        }
    }
}