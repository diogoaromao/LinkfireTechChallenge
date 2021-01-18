namespace SpotifyClient.Core.Config
{
    public class SpotifyClientConfig : ISpotifyClientConfig
    {
        public string Endpoint { get; }
        public string AccessToken { get; }

        public SpotifyClientConfig(string endpoint, string accessToken)
        {
            Endpoint = endpoint;
            AccessToken = accessToken;
        }
    }
}
