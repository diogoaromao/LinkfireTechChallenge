using LinkfireTechChallenge.Core.Models.Domain;
using LinkfireTechChallenge.Core.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace LinkfireTechChallenge.Core.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepo;

        public PlaylistService(IPlaylistRepository playlistRepo)
        {
            _playlistRepo = playlistRepo;
        }

        public async Task<Playlist> Get(string playlistId)
        {
            return await _playlistRepo.Get(playlistId);
        }

        public async Task AddTopNTracks(ArtistTopTracks artistTopTracks, string playlistId, int count)
        {
            var topTracks = artistTopTracks.Tracks.Take(count).Select(t => t.Uri);
            await _playlistRepo.AddTracks(topTracks, playlistId);
        }

        public async Task<int> GetTotalSongsInPlaylist(string playlistId)
        {
            return await _playlistRepo.GetTotalSongsInPlaylist(playlistId);
        }
    }
}
