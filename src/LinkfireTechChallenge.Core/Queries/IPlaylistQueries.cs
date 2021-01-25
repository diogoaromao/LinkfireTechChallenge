using LinkfireTechChallenge.Core.Models.DTO;
using System.Threading.Tasks;

namespace LinkfireTechChallenge.Core.Queries
{
    public interface IPlaylistQueries
    {
        Task<int> GetTotalSongsInPlaylist(QueryTotalSongsPlaylistDTO searchModel);
    }
}
