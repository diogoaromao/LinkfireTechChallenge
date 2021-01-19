using LinkfireTechChallenge.Core.Models.Domain;
using LinkfireTechChallenge.Core.Repositories;
using System.Threading.Tasks;

namespace LinkfireTechChallenge.Core.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepo;

        public ArtistService(IArtistRepository artistRepo)
        {
            _artistRepo = artistRepo;
            
        }

        public async Task<ArtistTopTracks> GetTopTracks(string artistId)
        {
            return await _artistRepo.GetTopTracks(artistId);
        }
    }
}
