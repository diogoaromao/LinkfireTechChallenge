using LinkfireTechChallenge.Core.Commands.Core;

namespace LinkfireTechChallenge.Core.Models.DTO
{
    public class QueryTotalSongsPlaylistDTO : IQueryModel
    {
        public string PlaylistId { get; set; }

        public QueryTotalSongsPlaylistDTO(string playlistId)
        {
            PlaylistId = playlistId;
        }
    }
}
