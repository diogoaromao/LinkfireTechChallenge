using AutoMapper;
using SpotifyClient.Core.DTO;
using SpotifyClient.Core.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkfireTechChallenge.Core.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly IMapper _mapper;
        private readonly ISpotifyClient _spotifyClient;

        public PlaylistRepository(IMapper mapper, ISpotifyClient spotifyClient)
        {
            _mapper = mapper;
            _spotifyClient = spotifyClient;
        }

        public async Task<string> Get(string playlistId)
        {
            return await _spotifyClient.GetPlaylist(playlistId);
        }

        public async Task AddTracks(IEnumerable<string> topTracks, string playlistId)
        {
            var tracksToAdd = _mapper.Map<IEnumerable<string>, AddTracksToPlaylistDTO>(topTracks);
            await _spotifyClient.AddTracksToPlaylist(tracksToAdd, playlistId);
        }
    }
}
