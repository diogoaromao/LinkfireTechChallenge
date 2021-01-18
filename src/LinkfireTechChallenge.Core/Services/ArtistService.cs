using SpotifyClient.Core.Models;
using SpotifyClient.Core.Utils;
using System.Threading.Tasks;

namespace LinkfireTechChallenge.Core.Services
{
    public class ArtistService : IArtistService
    {
        private readonly ISpotifyClient _spotifyClient;

        public ArtistService(ISpotifyClient spotifyClient)
        {
            _spotifyClient = spotifyClient;
        }

        public async Task<ArtistTopTracks> GetTopTracks(string artistId)
        {
            return await _spotifyClient.GetArtistTopTracks(artistId);
        }
    }
}
