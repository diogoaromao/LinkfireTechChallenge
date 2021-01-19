using LinkfireTechChallenge.Core.Models.Domain;
using System.Threading.Tasks;

namespace LinkfireTechChallenge.Core.Services
{
    public interface IPlaylistService
    {
        Task<string> Get(string playlistId);
        Task AddTopNTracks(ArtistTopTracks artistTopTracks, string playlistId, int count);
    }
}
