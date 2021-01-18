using Newtonsoft.Json;
using Polly;
using Spotify.Client.Core.Utils;
using SpotifyClient.Core.Config;
using SpotifyClient.Core.Extensions;
using SpotifyClient.Core.Models;
using SpotifyClient.Core.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SpotifyClient
{
    public sealed class SpotifyClient : ISpotifyClient, IDisposable
    {
        private const string MediaType = "application/json";
        private const string Bearer = "Bearer";
        private readonly HttpClient _httpClient;

        public SpotifyClient(ISpotifyClientConfig spotifyClientConfig)
        {
            if (spotifyClientConfig == null)
            {
                throw new ArgumentNullException(nameof(spotifyClientConfig));
            }

            if (string.IsNullOrWhiteSpace(spotifyClientConfig.Endpoint))
            {
                throw new ArgumentNullException(spotifyClientConfig.Endpoint);
            }

            if (string.IsNullOrWhiteSpace(spotifyClientConfig.AccessToken))
            {
                throw new ArgumentNullException(spotifyClientConfig.AccessToken);
            }

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(spotifyClientConfig.Endpoint)
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, spotifyClientConfig.AccessToken);
        }

        public async Task<ArtistTopTracks> GetArtistTopTracks(string artistId)
        {
            HttpResponseMessage response = null;
            try
            {
                var requestUri = $"artists/{artistId}/top-tracks?market=pt";

                await ExecuteTransientCall(async () =>
                {
                    response = await _httpClient.SendGetAsync(requestUri);
                });

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        var content = await response.Content.ReadAsStringAsync();
                        var tracks = JsonConvert.DeserializeObject<ArtistTopTracks>(content);
                        return tracks;
                    case HttpStatusCode.NotFound:
                        return null;
                    default:
                        throw new InvalidOperationException(response.ReasonPhrase);
                }
            }
            finally
            {
                response?.Dispose();
            }
        }

        public async Task<string> GetPlaylist(string playlistId)
        {
            HttpResponseMessage response = null;
            try
            {
                var requestUri = $"playlists/{playlistId}";

                await ExecuteTransientCall(async () =>
                {
                    response = await _httpClient.SendGetAsync(requestUri);
                });

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        var content = await response.Content.ReadAsStringAsync();
                        return content;
                    case HttpStatusCode.NotFound:
                        return null;
                    default:
                        throw new InvalidOperationException(response.ReasonPhrase);
                }
            }
            finally
            {
                response?.Dispose();
            }
        }

        public async Task AddTracksToPlaylist(string playlistId, IEnumerable<string> tracks)
        {

        }

        private T ExecuteTransientCall<T>(Func<T> func)
        {
            var policy = Policy.Handle<TimeoutException>()
                                .Or<WebException>(ex => ex.GetExceptionStatusCode() == HttpStatusCode.ServiceUnavailable)
                                .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            return policy.Execute(func);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        ~SpotifyClient()
        {
            Dispose();
        }
    }
}
