using System.Collections.Generic;

namespace SpotifyClient.Core.DTO
{
    public class ArtistTopTracksDTO
    {
        public IEnumerable<TrackDTO> Tracks { get; set; }
    }
}
