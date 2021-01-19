using Newtonsoft.Json;
using Polly;
using Spotify.Client.Core.Utils;
using SpotifyClient.Core.Config;
using SpotifyClient.Core.DTO;
using SpotifyClient.Core.Extensions;
using SpotifyClient.Core.Utils;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

        public async Task<ArtistTopTracksDTO> GetArtistTopTracks(string artistId)
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
                        var tracks = JsonConvert.DeserializeObject<ArtistTopTracksDTO>(content);
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

        public async Task AddTracksToPlaylist(AddTracksToPlaylistDTO tracks, string playlistId)
        {
            StringContent content = null;
            HttpResponseMessage response = null;
            try
            {
                var requestUri = $"playlists/{playlistId}/tracks";
                content = new StringContent(JsonConvert.SerializeObject(tracks), Encoding.UTF8, MediaType);

                await ExecuteTransientCall(async () =>
                {
                    response = await _httpClient.SendPostAsync(requestUri, content);
                });

                switch(response.StatusCode)
                {
                    case HttpStatusCode.Created:
                        return;
                    default:
                        throw new InvalidOperationException(response.ReasonPhrase);
                }
            }
            finally
            {
                content?.Dispose();
                response?.Dispose();
            }
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
