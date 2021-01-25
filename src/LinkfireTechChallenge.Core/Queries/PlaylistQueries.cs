using LinkfireTechChallenge.Core.Models.DTO;
using LinkfireTechChallenge.Core.Services;
using System.Threading.Tasks;

namespace LinkfireTechChallenge.Core.Queries
{
    public class PlaylistQueries : IPlaylistQueries
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistQueries(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        public async Task<int> GetTotalSongsInPlaylist(QueryTotalSongsPlaylistDTO searchModel)
        {
            return await _playlistService.GetTotalSongsInPlaylist(searchModel.PlaylistId);
        }
    }
}
