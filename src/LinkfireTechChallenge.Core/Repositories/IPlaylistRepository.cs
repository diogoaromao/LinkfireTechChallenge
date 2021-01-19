using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkfireTechChallenge.Core.Repositories
{
    public interface IPlaylistRepository
    {
        Task<string> Get(string playlistId);
        Task AddTracks(IEnumerable<string> topTracks, string playlistId);
    }
}
