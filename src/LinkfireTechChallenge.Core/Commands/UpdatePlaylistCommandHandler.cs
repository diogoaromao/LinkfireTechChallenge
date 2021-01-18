using LinkfireTechChallenge.Core.Commands.Core;
using LinkfireTechChallenge.Core.Services;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LinkfireTechChallenge.Core.Commands
{
    public class UpdatePlaylistCommandHandler : IRequestHandler<UpdatePlaylistCommand, CommandResult>
    {
        private readonly IArtistService _artistService;
        private readonly IPlaylistService _playlistService;

        public UpdatePlaylistCommandHandler(IArtistService artistService, IPlaylistService playlistService)
        {
            _artistService = artistService;
            _playlistService = playlistService;
        }

        public async Task<CommandResult> Handle(UpdatePlaylistCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var playlist = await _playlistService.Get(request.PlaylistId);
                if(playlist == null)
                {
                    return CommandResult.NotFound(nameof(playlist), request.PlaylistId);
                }

                var artistTopTracks = await _artistService.GetTopTracks(request.ArtistId);
                if (artistTopTracks == null)
                {
                    return CommandResult.NotFound(nameof(artistTopTracks), request.ArtistId);
                }

                return CommandResult.Ok();
            }
            catch(Exception ex)
            {
                return CommandResult.FromException(ex);
            }
        }
    }
}
