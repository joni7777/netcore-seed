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