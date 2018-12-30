namespace Bp.Logging.Sinks.Mattermost
{
    public class BpMattermostInfo
    {
        public string Host { get; }
        public string Path { get; }

        public BpMattermostInfo(string host, string path)
        {
            Host = host;
            Path = path;
        }
    }
}