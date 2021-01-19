using AutoMapper;
using LinkfireTechChallenge.Core.Models.Domain;
using SpotifyClient.Core.DTO;
using SpotifyClient.Core.Utils;
using System.Threading.Tasks;

namespace LinkfireTechChallenge.Core.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly IMapper _mapper;
        private readonly ISpotifyClient _spotifyClient;

        public ArtistRepository(IMapper mapper, ISpotifyClient spotifyClient)
        {
            _mapper = mapper;
            _spotifyClient = spotifyClient;
        }

        public async Task<ArtistTopTracks> GetTopTracks(string artistId)
        {
            return _mapper.Map<ArtistTopTracksDTO, ArtistTopTracks>(
                await _spotifyClient.GetArtistTopTracks(artistId));
        }
    }
}
