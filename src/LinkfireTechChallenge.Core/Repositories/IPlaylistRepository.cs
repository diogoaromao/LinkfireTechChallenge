using LinkfireTechChallenge.Core.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkfireTechChallenge.Core.Repositories
{
    public interface IPlaylistRepository
    {
        Task<Playlist> Get(string playlistId);
        Task AddTracks(IEnumerable<string> topTracks, string playlistId);
        Task<int> GetTotalSongsInPlaylist(string playlistId);
    }
}
