using System.Net.Http;
using System.Text;
using Bp.Logging.Sinks.Http;
using Serilog.Events;

namespace Bp.Logging.Sinks.Mattermost
{
    public class BpMattermostSink:BpHttpSink
    {
        private readonly BpMattermostInfo _bpMattermostInfo;

        public BpMattermostSink(BpMattermostInfo bpMattermostInfo, HttpClient httpClient = null) 
            : base(httpClient ?? new HttpClient())
        {
            _bpMattermostInfo = bpMattermostInfo;
        }

        protected override string GenerateRequestUri(LogEvent logEvent)
        {
            return $"https://{_bpMattermostInfo.Host}/{_bpMattermostInfo.Path}";
        }

        protected override HttpContent GenerateRequestBody(LogEvent logEvent)
        {
            logEvent.Properties.TryGetValue("ServiceName", out var serviceName);

            var mattermostMessage = BpMattermostLogFormatter.Format(logEvent);

            var builder = new StringBuilder();
            builder.Append('{');
            builder.Append("\"username\":\"").Append(serviceName).Append("\",");
            builder.Append("\"text\":\"").Append(mattermostMessage).Append("\"");
            builder.Append("\"");
            builder.Append('}');
            return new StringContent(builder.ToString(), Encoding.UTF8, "application/json");
        }
    }
}