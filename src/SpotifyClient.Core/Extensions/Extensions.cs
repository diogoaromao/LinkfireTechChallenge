using System;
using System.Net;

namespace SpotifyClient.Core.Extensions
{
    public static class Extensions
    {
        public static HttpStatusCode? GetExceptionStatusCode(this Exception exception)
        {
            HttpStatusCode? statusCode = null;

            if (exception.GetType().IsAssignableFrom(typeof(WebException)))
            {
                WebException webException = exception as WebException;

                HttpWebResponse response = webException.Response as HttpWebResponse;
                statusCode = response?.StatusCode;
            }

            return statusCode;
        }
    }
}
