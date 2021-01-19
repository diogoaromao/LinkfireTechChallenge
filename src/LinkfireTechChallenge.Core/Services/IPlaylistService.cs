using LinkfireTechChallenge.Core.Models.Domain;
using System.Threading.Tasks;

namespace LinkfireTechChallenge.Core.Services
{
    public interface IPlaylistService
    {
        Task<Playlist> Get(string playlistId);
        Task AddTopNTracks(ArtistTopTracks artistTopTracks, string playlistId, int count);
        Task<int> GetTotalSongsInPlaylist(string playlistId);
    }
}
