using LinkfireTechChallenge.Core.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LinkfireTechChallenge.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class PlaylistsController
    {
        private readonly IMediator _mediator;

        public PlaylistsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("AddTopNTracks/{artistId}/{playlistId}/{count}")]
        public async Task<IActionResult> AddTopNTracks(string artistId, string playlistId, int count)
        {
            return await _mediator.Send(new UpdatePlaylistCommand(artistId, playlistId, count));
        }
    }
}
