using System.Threading.Tasks;

namespace LinkfireTechChallenge.Core.Services
{
    public interface IPlaylistService
    {
        Task<string> Get(string playlistId);
        Task Update(string artistId, string playlistId, int count);
    }
}
