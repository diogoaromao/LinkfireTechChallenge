using LinkfireTechChallenge.Core.Commands.Core;

namespace LinkfireTechChallenge.Core.Commands
{
    public class UpdatePlaylistCommand : ICommand<CommandResult>
    {
        public string  ArtistId { get; set; }
        public string PlaylistId { get; set; }
        public int Count { get; set; }

        public UpdatePlaylistCommand(string artistId, string playlistId, int count)
        {
            ArtistId = artistId;
            PlaylistId = playlistId;
            Count = count;
        }
    }
}
