using AutoMapper;
using LinkfireTechChallenge.Core.Commands;
using LinkfireTechChallenge.Core.Models.DTO;
using LinkfireTechChallenge.Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LinkfireTechChallenge.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class PlaylistsController : BaseController
    {
        private readonly IPlaylistQueries _playlistQueries;

        public PlaylistsController(IMediator mediator, IMapper mapper, IPlaylistQueries playlistQueries) 
            : base(mediator, mapper)
        {
            _playlistQueries = playlistQueries;
        }

        [HttpPost("AddTopNTracks/{artistId}/{playlistId}/{n}")]
        public async Task<IActionResult> AddTopNTracks(string artistId, string playlistId, int n)
        {
            return await _mediator.Send(new UpdatePlaylistCommand(artistId, playlistId, n));
        }

        [HttpGet("GetTotalSongs/{playlistId}")]
        public async Task<IActionResult> GetTotalSongs(string playlistId)
        {
            var searchModel = new QueryTotalSongsPlaylistDTO(playlistId);
            return await Retrieve(searchModel,
                () => _playlistQueries.GetTotalSongsInPlaylist(searchModel));
        }
    }
}
