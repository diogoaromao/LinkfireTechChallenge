using System.Net.Http;
using System.Threading.Tasks;

namespace Spotify.Client.Core.Utils
{
    public static class HttpClientUtils
    {
        private const string HTTP_METHOD_POST = "POST";

        public static Task<HttpResponseMessage> SendGetAsync(this HttpClient client, string requestUri)
            => client.SendRequest(HttpMethod.Get, requestUri, null);

        public static Task<HttpResponseMessage> SendPostAsync(this HttpClient client, string requestUri, HttpContent content)
            => client.SendRequest(new HttpMethod(HTTP_METHOD_POST), requestUri, content);

        internal static Task<HttpResponseMessage> SendRequest(this HttpClient client, HttpMethod method, string requestUri, HttpContent content)
        {
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };

            var response = client.SendAsync(request);
            return response;
        }
    }
}
