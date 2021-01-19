using System.Collections.Generic;

namespace SpotifyClient.Core.DTO
{
    public class AddTracksToPlaylistDTO
    {
        public IEnumerable<string> Uris { get; set; }
    }
}
