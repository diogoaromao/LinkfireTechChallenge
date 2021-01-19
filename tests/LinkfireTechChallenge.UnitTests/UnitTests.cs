using LinkfireTechChallenge.Core.Commands;
using LinkfireTechChallenge.Core.Validators;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace LinkfireTechChallenge.UnitTests
{
    [TestFixture]
    public class UnitTests
    {
        private UpdatePlaylistCommandValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new UpdatePlaylistCommandValidator();

        }

        [TestCase("", "", 0)]
        [TestCase("artist", "", 0)]
        [TestCase("", "playlist", 0)]
        [TestCase("", "", 1)]
        [TestCase("artist", "", 1)]
        [TestCase("artist", "playlist", 0)]
        [TestCase("", "playlist", 1)]
        public async Task AddTracksToPlaylistInvalidArguments_ReturnsError(string artistId, string playlistId, int count)
        {
            // Act
            var result = await validator.ValidateAsync(new UpdatePlaylistCommand(artistId, playlistId, count));

            // Assert
            Assert.IsTrue(result.Errors.Any());
        }

        [TestCase("artist", "playlist", 1)]
        public async Task AddTracksToPlaylist_ValidArguments_ReturnsNoError(string artistId, string playlistId, int count)
        {
            // Act
            var result = await validator.ValidateAsync(new UpdatePlaylistCommand(artistId, playlistId, count));

            // Assert
            Assert.IsFalse(result.Errors.Any());
        }
    }
}