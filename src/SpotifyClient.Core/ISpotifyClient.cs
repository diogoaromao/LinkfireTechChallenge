using SpotifyClient.Core.DTO;
using System.Threading.Tasks;

namespace SpotifyClient.Core.Utils
{
    public interface ISpotifyClient
    {
        Task<string> GetPlaylist(string playlistId);
        Task<ArtistTopTracksDTO> GetArtistTopTracks(string artistId);
        Task AddTracksToPlaylist(AddTracksToPlaylistDTO tracks, string playlistId);
    }
}
