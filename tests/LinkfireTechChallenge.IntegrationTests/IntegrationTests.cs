using AutoMapper;
using LinkfireTechChallenge.Core.Commands;
using LinkfireTechChallenge.Core.Models.Domain;
using LinkfireTechChallenge.Core.Repositories;
using LinkfireTechChallenge.Core.Services;
using LinkfireTechChallenge.IntegrationTests.Model.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using SpotifyClient.Core.Config;
using SpotifyClient.Core.DTO;
using SpotifyClient.Core.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LinkfireTechChallenge.IntegrationTests
{
    [TestFixture]
    public class IntegrationTests
    {
        private ISpotifyClient spotifyClient;
        private IMapper mapper;
        private IArtistRepository artistRepository;
        private IPlaylistRepository playlistRepository;
        private IArtistService artistService;
        private IPlaylistService playlistService;
        private UpdatePlaylistCommandHandler handler;
        private static AppSettings settings;

        [SetUp]
        public void Setup()
        {
            InitializeAppSettings();
            InitializeMapper();
            InitializeDI();
            InitializeJsonConvert();
        }

        private void InitializeJsonConvert()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        private void InitializeDI()
        {
            spotifyClient = new SpotifyClient.SpotifyClient(new SpotifyClientConfig(settings.Endpoint, settings.AccessToken));
            artistRepository = new ArtistRepository(mapper, spotifyClient);
            playlistRepository = new PlaylistRepository(mapper, spotifyClient);
            artistService = new ArtistService(artistRepository);
            playlistService = new PlaylistService(playlistRepository);
            handler = new UpdatePlaylistCommandHandler(artistService, playlistService);
        }

        private void InitializeMapper()
        {
            mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TrackDTO, Track>();
                cfg.CreateMap<ArtistTopTracksDTO, ArtistTopTracks>();
                cfg.CreateMap<IEnumerable<string>, AddTracksToPlaylistDTO>()
                    .ForMember(dest => dest.Uris,
                                opt => opt.MapFrom(src => src));
                cfg.CreateMap<PlaylistDTO, Playlist>();
            }));
        }

        private static void InitializeAppSettings()
        {
            using var reader = new StreamReader(Directory.GetCurrentDirectory() + "/appSettings.json");
            settings = JsonConvert.DeserializeObject<AppSettings>(reader.ReadToEnd());
        }

        [TestCase("artistId", "2V2kViZ3xY2oEUW6nSMSMn", 1)]
        public async Task AddTracksToPlaylist_InvalidArguments_ReturnsBadRequest(string artistId, string playlistId, int count)
        {
            // Act
            var result = await handler.Handle(new UpdatePlaylistCommand(artistId, playlistId, count), new CancellationToken(false));

            // Assert
            Assert.IsTrue(result.Errors.Any());
            Assert.AreEqual("Bad Request", result.Errors.First().Message);
        }

        [TestCase("artist", "playlist", 1)]
        public async Task AddTracksToPlaylist_InvalidArguments_ReturnsNotFound(string artistId, string playlistId, int count)
        {
            // Act
            var result = await handler.Handle(new UpdatePlaylistCommand(artistId, playlistId, count), new CancellationToken(false));

            // Assert
            Assert.IsTrue(result.Errors.Any());
            Assert.AreEqual("NotFound", result.Errors.First().Title);
        }

        [TestCase("artistId", "5if30uTOUlJoRVXDSjrKlk", 1)]
        public async Task AddTracksToNonCollaborativePlaylist_ValidArguments_ReturnsDefaultError(string artistId, string playlistId, int count)
        {
            // Act
            var result = await handler.Handle(new UpdatePlaylistCommand(artistId, playlistId, count), new CancellationToken(false));

            // Assert
            Assert.IsTrue(result.Errors.Any());
            Assert.AreEqual("DefaultError", result.Errors.First().Title);
        }

        [TestCase("12Chz98pHFMPJEknJQMWvI", "2V2kViZ3xY2oEUW6nSMSMn", 10)]

        public async Task AddTracksToPlaylist_ValidArguments_Success(string artistId, string playlistId, int count)
        {
            // Act
            var totalSongs = await playlistService.GetTotalSongsInPlaylist(playlistId);
            await handler.Handle(new UpdatePlaylistCommand(artistId, playlistId, count), new CancellationToken(false));

            count += totalSongs;
            totalSongs = await playlistService.GetTotalSongsInPlaylist(playlistId);

            // Assert
            Assert.AreEqual(count, totalSongs);
        }
    }
}