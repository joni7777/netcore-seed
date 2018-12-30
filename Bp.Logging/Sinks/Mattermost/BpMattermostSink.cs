using System.Net.Http;
using System.Text;
using Bp.Logging.Sinks.Http;
using Serilog.Events;

namespace Bp.Logging.Sinks.Mattermost
{
    public class BpMattermostSink:BpHttpSink
    {
        private readonly BpMattermostInfo _bpMattermostInfo;
        private readonly BpLogFormatter _formatter;

        public BpMattermostSink(BpMattermostInfo bpMattermostInfo, BpLogFormatter formatter = null, HttpClient httpClient = null) 
            : base(httpClient ?? new HttpClient())
        {
            _bpMattermostInfo = bpMattermostInfo;
            _formatter = formatter ?? new BpMattermostLogFormatter();
        }

        protected override string GenerateRequestUri(LogEvent logEvent)
        {
            return $"https://{_bpMattermostInfo.Host}/{_bpMattermostInfo.Path}";
        }

        protected override HttpContent GenerateRequestBody(LogEvent logEvent)
        {
            logEvent.Properties.TryGetValue("ServiceName", out var serviceName);

            var mattermostMessage = _formatter.Format(logEvent);

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