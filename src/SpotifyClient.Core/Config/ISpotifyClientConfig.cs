namespace SpotifyClient.Core.Config
{
    public interface ISpotifyClientConfig
    {
        string Endpoint { get; }
        string AccessToken { get; }
    }
}
