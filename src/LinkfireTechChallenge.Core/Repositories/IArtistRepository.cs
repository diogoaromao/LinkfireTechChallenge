using LinkfireTechChallenge.Core.Models.Domain;
using System.Threading.Tasks;

namespace LinkfireTechChallenge.Core.Repositories
{
    public interface IArtistRepository
    {
        Task<ArtistTopTracks> GetTopTracks(string artistId);
    }
}
