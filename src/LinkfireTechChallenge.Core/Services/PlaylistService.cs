using SpotifyClient.Core.Utils;
using System.Threading.Tasks;

namespace LinkfireTechChallenge.Core.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly ISpotifyClient _spotifyClient;

        public PlaylistService(ISpotifyClient spotifyClient)
        {
            _spotifyClient = spotifyClient;
        }

        public async Task<string> Get(string playlistId)
        {
            return await _spotifyClient.GetPlaylist(playlistId);
        }

        public async Task Update(string artistId, string playlistId, int count)
        {
            var playlist = await _spotifyClient.GetPlaylist(playlistId);
            
        }
    }
}
