using SpotifyClient.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyClient.Core.Utils
{
    public interface ISpotifyClient
    {
        Task<string> GetPlaylist(string playlistId);
        Task<ArtistTopTracks> GetArtistTopTracks(string artistId);
        Task AddTracksToPlaylist(string playlistId, IEnumerable<string> tracks);
    }
}
