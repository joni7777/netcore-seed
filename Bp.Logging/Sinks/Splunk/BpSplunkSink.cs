using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using Bp.Logging.Sinks.Http;
using Serilog.Events;

namespace Bp.Logging.Sinks.Splunk
{
    public class BpSplunkSink : BpBulkHttpSink
    {
        private readonly string _splunkUrl;
        private readonly BpSplunkInfo _splunkInfo;
        private readonly BpLogFormatter _formater;
        private AuthenticationHeaderValue _basicAuth;

        public BpSplunkSink(BpSplunkInfo splunkInfo, BpLogFormatter formatter = null, ConcurrentBag<LogEvent> logs = null, Timer timer = null,
            HttpClient httpClient = null)
            : base(httpClient ?? new HttpClient(), timer, logs)
        {
            _basicAuth = CreateBasicAuth(splunkInfo);
            _splunkInfo = splunkInfo;
            _formater = formatter ?? new BpSplunkLogFormatter();
        }

        protected override void ConfigureHttpRequest(HttpRequestMessage request)
        {
            request.Headers.Authorization = _basicAuth;
        }

        private AuthenticationHeaderValue CreateBasicAuth(BpSplunkInfo splunkInfo)
        {
            string usernamePassword = $"{splunkInfo.Username}:{splunkInfo.Password}";
            byte[] encodedBytes = Encoding.Unicode.GetBytes(usernamePassword);

            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(encodedBytes));
        }

        protected override string GenerateRequestUri(LogEvent logEvent)
        {
            logEvent.Properties.TryGetValue("ServiceName", out var serviceName);
            return
                $"https://{_splunkInfo.Host}:{_splunkInfo.Port}/services/receivers/simple?index={_splunkInfo.Index}&sourcetype={_splunkInfo.SourceType}&source={serviceName}&output_mode=json";
        }

        protected override HttpContent GenerateRequestBody(ConcurrentBag<LogEvent> logs)
        {
            var joinedLogs = "";

            foreach (LogEvent log in logs)
            {
                joinedLogs += string.Format("{0}{0}", _formater.Format(log), Environment.NewLine);
            }

            return new StringContent(joinedLogs, Encoding.UTF8, "application/json");
        }
    }
}