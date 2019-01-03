using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BpSeed.Common.Extensions
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> SendAsyncAsJSon(this HttpClient client, HttpRequestMessage request)
        {
            request.Content = new JsonContent(request.Content);
            return client.SendAsync(request);
        }

        public static Task<HttpResponseMessage> SendAsyncAsJSon(this HttpClient client, HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            request.Content = new JsonContent(request.Content);
            return client.SendAsync(request, cancellationToken);
        }

        public static Task<HttpResponseMessage> SendAsyncAsJSon(this HttpClient client, HttpRequestMessage request, HttpCompletionOption completionOption)
        {
            request.Content = new JsonContent(request.Content);
            return client.SendAsync(request, completionOption);
        }
    
        public static Task<HttpResponseMessage> SendAsyncAsJSon(this HttpClient client, HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            request.Content = new JsonContent(request.Content);
            return client.SendAsync(request, completionOption, cancellationToken);
        }
    }
}